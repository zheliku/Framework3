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
    using UnityEngine;
    using UnityEngine.InputSystem;

    public enum InputType
    {
        Press,
        Hold,
        Release
    }

    public partial class OldInputKit
    {
        public static bool EnableAxis { get; set; } = true;

        public static bool EnableMouse { get; set; } = true;

        public static bool EnableKeyCode { get; set; } = true;

        public static bool EnableAll
        {
            get => EnableKeyCode && EnableMouse && EnableAxis;
            set
            {
                EnableKeyCode = value;
                EnableMouse   = value;
                EnableAxis    = value;
            }
        }

        public static void RegisterAxis(string axisName, Action<float, float> action)
        {
            AxisInput.Instance.Register(axisName, action);
        }

        public static void RegisterHorizontalAndVertical(Action<Vector2, Vector2> action, bool isRaw = false)
        {
            AxisInput.Instance.RegisterHorizontalAndVertical(action, isRaw);
        }

        public static void UnregisterAxis(string axisName, Action<float, float> action)
        {
            AxisInput.Instance.Unregister(axisName, action);
        }

        public static void UnregisterAxis(string axisName)
        {
            AxisInput.Instance.Unregister(axisName);
        }

        public static void UnregisterAxisAll()
        {
            AxisInput.Instance.UnregisterAll();
        }

        public void UnregisterHorizontalAndVertical(Action<Vector2, Vector2> action, bool isRaw = false)
        {
            AxisInput.Instance.UnregisterHorizontalAndVertical(action, isRaw);
        }

        public void UnregisterHorizontalAndVertical(bool isRaw)
        {
            AxisInput.Instance.UnregisterHorizontalAndVertical(isRaw);
        }

        public void UnregisterHorizontalAndVertical()
        {
            AxisInput.Instance.UnregisterHorizontalAndVertical();
        }

        public static void RegisterMouse(MouseInputType mouseType, Action<bool, bool> action, InputType inputType = InputType.Press)
        {
            MouseInput.Instance.Register(mouseType, action, inputType);
        }

        public static void UnregisterMouse(MouseInputType mouseType, Action<bool, bool> action, InputType inputType = InputType.Press)
        {
            MouseInput.Instance.Unregister(mouseType, action, inputType);
        }

        public static void UnregisterMouse(MouseInputType mouseType, InputType inputType)
        {
            MouseInput.Instance.Unregister(mouseType, inputType);
        }

        public static void UnregisterMouse(MouseInputType mouseType)
        {
            MouseInput.Instance.Unregister(mouseType);
        }

        public static void UnregisterMouseAll()
        {
            MouseInput.Instance.UnregisterAll();
        }

        public static void RegisterKeyCode(KeyCode keyCode, Action<bool, bool> action, InputType inputType = InputType.Press)
        {
            KeyCodeInput.Instance.Register(keyCode, action, inputType);
        }

        public static void UnregisterKeyCode(KeyCode keyCode, Action<bool, bool> action, InputType inputType = InputType.Press)
        {
            KeyCodeInput.Instance.Unregister(keyCode, action, inputType);
        }

        public static void UnregisterKeyCode(KeyCode keyCode, InputType inputType)
        {
            KeyCodeInput.Instance.Unregister(keyCode, inputType);
        }

        public static void UnregisterKeyCode(KeyCode keyCode)
        {
            KeyCodeInput.Instance.Unregister(keyCode);
        }

        public static void UnregisterKeyCodeAll()
        {
            KeyCodeInput.Instance.UnregisterAll();
        }
    }
}