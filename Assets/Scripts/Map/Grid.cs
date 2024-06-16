using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Map
{
    public class Grid : MonoBehaviour
    {
        private GridInfo _info; // 即地块的确定状态，为空时说明状态未确定
        private List<GridInfo> _states;
        
        [SerializeField] private float width;
        [SerializeField] private float height;
        
        public GridInfo Info => _info;
        public List<GridInfo> States => _states;
        public SpriteRenderer TheSpriteRenderer => GetComponent<SpriteRenderer>();
        
        /// <summary>
        /// 调整地块位置，初始化状态列表
        /// </summary>
        public void Init(int x,int y)
        {
            transform.position = new Vector3(x * width, y * height);
            _states = DataManager.Instance.GetAllInfo();
        }

        /// <summary>
        /// 确定地块状态
        /// </summary>
        public void WaveFunctionCollapse()
        {
            _info = _states[Random.Range(0, _states.Count)];
            _states.Clear();
            UpdateSprite();
        }
        
        /// <summary>
        /// 更新状态列表为原列表与输入列表的交集，如果此时状态列表仅有一个状态，则地块的状态确定
        /// </summary>
        public void WaveFunctionCollapse(List<GridInfo> infos)
        {
            _states = _states.Intersect(infos).ToList();
            if(_states.Count == 1)WaveFunctionCollapse();
        }
        
        private void UpdateSprite()
        {
            if (_info is null) return;
            TheSpriteRenderer.sprite = DataManager.Instance.GetGridDataWithID(_info.id).Sprite;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}