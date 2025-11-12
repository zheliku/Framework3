// ------------------------------------------------------------
// @file       DynamicGridExample.cs
// @brief
// @author     zheliku
// @Modified   2024-11-01 13:11:44
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.GridKit.Example._1.DynamicGrid
{
    #if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
    using UnityEngine;

    public class DynamicGridExample : MonoBehaviour
    {
        #if ODIN_INSPECTOR
[ShowInInspector]
#endif
        private DynamicGrid<string> _grid;

        private void Start()
        {
            _grid = new DynamicGrid<string>();
        }

        public void SetValue()
        {
            _grid[2, 3] = "@@@ Hello @@@";
        }
        
        public void LogAll()
        {
            _grid.ForEach((i, j, value) => Debug.Log($"({i}, {j}) = {value}"));
        }
    }
}