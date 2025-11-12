// ------------------------------------------------------------
// @file       UnLoadTrigger.cs
// @brief
// @author     zheliku
// @Modified   2024-12-10 13:12:35
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class UnloadTrigger : MonoBehaviour
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly HashSet<AsyncOperationHandle> _handles = new();

        public AsyncOperationHandle AddHandle(AsyncOperationHandle handle)
        {
            _handles.Add(handle);
            return handle;
        }

        public void RemoveHandle(AsyncOperationHandle handle)
        {
            _handles.Remove(handle);
        }

        public void Unload()
        {
            foreach (var handle in _handles)
            {
                handle.Unload();
            }

            // 清空 HashSet
            _handles.Clear();
        }
    }
}