// ------------------------------------------------------------
// @file       InputActionTracker.cs
// @brief
// @author     zheliku
// @Modified   2025-08-27 01:02:05
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.InputSystem;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class InputActionTracker
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public List<Action<InputAction.CallbackContext>> CanceledActions = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public List<Action<InputAction.CallbackContext>> PerformedActions = new();
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public List<Action<InputAction.CallbackContext>> StartedActions = new();
    }
}