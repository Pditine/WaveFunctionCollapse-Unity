using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        private void Awake()
        {
            if (Instance is not null) return;
            Instance = this;
        }

        [SerializeField] private List<GridData> grids;

        public List<GridData> Grids => grids;

        public GridData GetGridDataWithID(int id)
        {
            return grids.Find(g => g.ID == id);
        }

        public List<GridInfo> GetAllInfo()
        {
            return grids.Select(grid => new GridInfo(grid.ID)).ToList();
        }
    }
}