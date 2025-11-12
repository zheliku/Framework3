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
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class InputActionMapTracker
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public Dictionary<string, InputActionTracker> InputActions = new();

        public InputActionTracker this[string actionName]
        {
            get => InputActions.GetValueOrDefault(actionName);
        }
    }
}