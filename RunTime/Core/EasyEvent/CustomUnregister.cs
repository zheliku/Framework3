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
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     自定义注销器
    /// </summary>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public struct CustomUnregister : IUnregister
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
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