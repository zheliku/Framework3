// ------------------------------------------------------------
// @file       CustomUnregister.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 16:10:04
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
    using Sirenix.OdinInspector;

    /// <summary>
    /// 自定义注销器
    /// </summary>
    [HideReferenceObjectPicker]
    public struct CustomUnregister : IUnregister
    {
        [ShowInInspector]
        private Action _onUnregister;

        public CustomUnregister(Action onUnregister)
        {
            _onUnregister = onUnregister;
        }

        public void Unregister()
        {
            _onUnregister?.Invoke();
            _onUnregister = null;
        }
    }
}