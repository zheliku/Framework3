// ------------------------------------------------------------
// @file       InputKit.cs
// @brief
// @author     zheliku
// @Modified   2024-12-03 14:12:30
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System;
    using UnityEngine.InputSystem;

    public class InputKit
    {
        public static InputAction GetInputAction(string actionName, string actionMapName = "Player")
        {
            return InputSystem.actions.FindActionMap(actionMapName)[actionName];
        }

        public static InputActionMap GetInputActionMap(string actionMapName)
        {
            return InputSystem.actions.FindActionMap(actionMapName);
        }

        public static InputAction RegisterPerformed(string actionName, Action<InputAction.CallbackContext> callback, string actionMapName = "Player")
        {
            return GetInputAction(actionName).RegisterPerformed(callback);
        }

        public static InputAction RegisterCanceled(string actionName, Action<InputAction.CallbackContext> callback, string actionMapName = "Player")
        {
            return GetInputAction(actionName).RegisterCanceled(callback);
        }

        public static InputAction RegisterStarted(string actionName, Action<InputAction.CallbackContext> callback, string actionMapName = "Player")
        {
            return GetInputAction(actionName).RegisterStarted(callback);
        }

        public static InputAction UnregisterPerformed(string actionName, Action<InputAction.CallbackContext> callback, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterPerformed(callback);
        }

        public static InputAction UnregisterCanceled(string actionName, Action<InputAction.CallbackContext> callback, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterCanceled(callback);
        }

        public static InputAction UnregisterStarted(string actionName, Action<InputAction.CallbackContext> callback, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterStarted(callback);
        }
        
        public static InputAction UnregisterAllPerformed(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterPerformedAll();
        }

        public static InputAction UnregisterAllCanceled(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterCanceledAll();
        }

        public static InputAction UnregisterAllStarted(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterStartedAll();
        }
        
        public static InputAction UnregisterAll(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).UnregisterAll();
        }

        public static TValue ReadValue<TValue>(string actionName, string actionMapName = "Player") where TValue : struct
        {
            return GetInputAction(actionName).ReadValue<TValue>();
        }

        public static bool WasCompletedThisFrame(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).WasCompletedThisFrame();
        }

        public static bool WasPerformedThisFrame(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).WasPerformedThisFrame();
        }
        
        public static bool WasPressedThisFrame(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).WasPressedThisFrame();
        }
        
        public static bool WasReleasedThisFrame(string actionName, string actionMapName = "Player")
        {
            return GetInputAction(actionName).WasReleasedThisFrame();
        }
    }
}