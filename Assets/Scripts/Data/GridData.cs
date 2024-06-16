using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public record GridInfo
    {
        public int id;

        public GridInfo(int id)
        {
            this.id = id;
        }
    }
    
    [CreateAssetMenu(fileName = "Grid", menuName = "Grid")]
    public class GridData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private List<GridInfo> upNeighbours;
        [SerializeField] private List<GridInfo> downNeighbours;
        [SerializeField] private List<GridInfo> leftNeighbours;
        [SerializeField] private List<GridInfo> rightNeighbours;
        [SerializeField] private Sprite sprite;

        public int ID => id;
        public List<GridInfo> UpNeighbours => upNeighbours;
        public List<GridInfo> DownNeighbours => downNeighbours;
        public List<GridInfo> LeftNeighbours => leftNeighbours;
        public List<GridInfo> RightNeighbours => rightNeighbours;
        public Sprite Sprite => sprite;
    }
}