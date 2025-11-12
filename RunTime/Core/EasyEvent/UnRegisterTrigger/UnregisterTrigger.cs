// ------------------------------------------------------------
// @file       UnregisterTrigger.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 16:10:29
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     注销触发器基类
    /// </summary>
    public abstract class UnregisterTrigger : MonoBehaviour
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly HashSet<IUnregister> _unRegisters = new(); // 存储 IUnregister 接口的实现类

        /// <summary>
        ///     添加注销器
        /// </summary>
        /// <param name="unregister">待添加的注销器</param>
        /// <returns>添加后的注销器</returns>
        public IUnregister AddUnregister(IUnregister unregister)
        {
            _unRegisters.Add(unregister);
            return unregister;
        }

        /// <summary>
        ///     移除注销器
        /// </summary>
        /// <param name="unregister">待移除的注销器</param>
        public void RemoveUnregister(IUnregister unregister)
        {
            _unRegisters.Remove(unregister);
        }

        /// <summary>
        ///     触发所有注销器
        /// </summary>
        public void Unregister()
        {
            foreach (var unRegister in _unRegisters)
            {
                unRegister.Unregister();
            }

            // 清空 HashSet
            _unRegisters.Clear();
        }
    }
}