// ------------------------------------------------------------
// @file       InputActionTracker.cs
// @brief
// @author     zheliku
// @Modified   2025-08-27 00:21:50
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;

    public class InputActionMapTracker
    {
        [ShowInInspector]
        public Dictionary<string, InputActionTracker> InputActions = new();

        public InputActionTracker this[string actionName]
        {
            get => InputActions.GetValueOrDefault(actionName);
        }
    }
}