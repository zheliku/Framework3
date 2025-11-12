// ------------------------------------------------------------
// @file       ListPoolExample.cs
// @brief
// @author     zheliku
// @Modified   2025-05-16 01:19:45
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit.Example._0.ObjectPoolExample
{
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class ListPoolExample : MonoBehaviour
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public ObjectPool<List<int>> ListPool = ListPool<int>.Pool;
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public List<List<int>> Lists = new();

        public void GetList()
        {
            Lists.Add(ListPool<int>.Get());
        }

        public void ReleaseList()
        {
            Lists[0].Release2Pool();
            Lists.RemoveAt(0);
        }

        public void ClearList()
        {
            ListPool<int>.Clear();
        }
    }
}