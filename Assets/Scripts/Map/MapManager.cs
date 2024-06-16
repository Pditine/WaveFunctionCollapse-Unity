using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        private Grid[,] _map; // 当前地图数据
        private readonly List<Grid> _inCollapsedGrids = new(); // 未坍缩的地块
        
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Grid gridPrototype; // 地块的预制体原型
        
        private void Start()
        {
            _map = new Grid[width, height];
            Init();
        }

        private void Init()
        {
            _inCollapsedGrids.Clear();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(_map[i,j] is not null)
                        _map[i,j].Destroy();
                    var theGrid = Instantiate(gridPrototype,transform).GetComponent<Grid>();
                    theGrid.Init(i,j);
                    _map[i, j] = theGrid;
                    _inCollapsedGrids.Add(theGrid);
                }
            }
        }

        public void CreateMap()
        {
            Init();
            for (int i = 0; i < width*height; i++)
            {
                CheckEntropy();
            }
        }
        
        public void CheckEntropy()
        {
            if (_inCollapsedGrids.Count == 0) return;
            List<Grid> tempGrid = new(_inCollapsedGrids);

            // 将剩余的未坍缩的地块按熵大小排列，熵最小的在前
            tempGrid.Sort((a, b) => { return a.States.Count - b.States.Count; }); 

            int arrLength = tempGrid[0].States.Count;
            int stopIndex = default;

            // 将熵大于最小值的地块去除
            for (int i = 1; i < tempGrid.Count; i++)
            {
                if (tempGrid[i].States.Count > arrLength)
                {
                    stopIndex = i;
                    break;
                }
            }
            if (stopIndex > 0)
            {
                tempGrid.RemoveRange(stopIndex, tempGrid.Count - stopIndex);
            }
            
            //输入目前熵最小的地块
            WaveFunctionCollapse(tempGrid);
            
            //更新所有地块的状态
            UpdateState();
        }

        /// <summary>
        /// 选取熵最小的地块进行坍塌
        /// </summary>
        private void WaveFunctionCollapse(List<Grid> grids)
        {
            var gridToCollapse = grids[Random.Range(0, grids.Count)];
            gridToCollapse.WaveFunctionCollapse();
        }
        
        /// <summary>
        /// 更新所有地块的状态，此时地块的状态有可能确定，需要更新未坍塌地块列表
        /// </summary>
        private void UpdateState()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(_map[i,j].Info is null)continue;
                    
                    if(i>0)
                        _map[i-1,j].WaveFunctionCollapse(DataManager.Instance.GetGridDataWithID(_map[i,j].Info.id).LeftNeighbours);
                    if(i<width-1)
                        _map[i+1,j].WaveFunctionCollapse(DataManager.Instance.GetGridDataWithID(_map[i,j].Info.id).RightNeighbours);
                    if(j>0)
                        _map[i,j-1].WaveFunctionCollapse(DataManager.Instance.GetGridDataWithID(_map[i,j].Info.id).DownNeighbours);
                    if(j<height-1)
                        _map[i,j+1].WaveFunctionCollapse(DataManager.Instance.GetGridDataWithID(_map[i,j].Info.id).UpNeighbours);
                }
            }
            _inCollapsedGrids.RemoveAll(c => c.Info is not null);
        }
    }
}