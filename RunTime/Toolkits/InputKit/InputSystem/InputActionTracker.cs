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
    using Sirenix.OdinInspector;
    using UnityEngine.InputSystem;

    public class InputActionTracker
    {
        [ShowInInspector]
        public List<Action<InputAction.CallbackContext>> StartedActions = new();

        [ShowInInspector]
        public List<Action<InputAction.CallbackContext>> PerformedActions = new();
        
        [ShowInInspector]
        public List<Action<InputAction.CallbackContext>> CanceledActions = new();
    }
}