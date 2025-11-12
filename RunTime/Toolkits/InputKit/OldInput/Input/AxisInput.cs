// ------------------------------------------------------------
// @file       AxisInput.cs
// @brief
// @author     zheliku
// @Modified   2024-08-23 23:08:33
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System;
    using System.Collections.Generic;
    using Core;
    using SingletonKit;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [MonoSingletonPath("Framework3/InputKit/AxisInput")]
    public class AxisInput : MonoSingleton<AxisInput>
    {
    #region Unity 事件

        protected override void Update()
        {
            base.Update();

            if (!OldInputKit.EnableAxis)
            {
                return;
            }

            foreach (var pair in _axisInputProperties)
            {
                var property = pair.Value;
                property.Value = property.IsRaw ? Input.GetAxisRaw(pair.Key) : Input.GetAxis(pair.Key);
            }

            _horizontalAndVerticalProperty.Value    = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _horizontalAndVerticalRawProperty.Value = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

    #endregion

    #region Static

        public static string Horizontal = "Horizontal";
        public static string Vertical   = "Vertical";
        public static string Jump       = "Jump";
        public static string MouseX     = "Mouse X";
        public static string MouseY     = "Mouse Y";

    #endregion

    #region 字段

    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelText("Axis")] [PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "Axis", ValueLabel = "Value")]
    #endif
        private Dictionary<string, BindableAxisInputProperty> _axisInputProperties = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private BindableTwoAxisInputProperty _horizontalAndVerticalProperty = new(false);

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private BindableTwoAxisInputProperty _horizontalAndVerticalRawProperty = new(true);

    #endregion

    #region 公共方法

        public void Register(string axisName, Action<float, float> action, bool isRaw = false)
        {
            if (!_axisInputProperties.TryGetValue(axisName, out var value))
            {
                value = new BindableAxisInputProperty(isRaw);

                _axisInputProperties[axisName] = value;
            }

            value.Register(action).UnregisterWhenGameObjectDestroyed(Instance);
        }

        public void Unregister(string axisName, Action<float, float> action)
        {
            if (_axisInputProperties.TryGetValue(axisName, out var value))
            {
                value.Unregister(action);

                if (value.EventCount == 0)
                {
                    _axisInputProperties.Remove(axisName);
                }
            }
        }

        public void Unregister(string axisName)
        {
            if (_axisInputProperties.TryGetValue(axisName, out var value))
            {
                value.UnregisterAll();
                _axisInputProperties.Remove(axisName);
            }
        }

        public void RegisterHorizontalAndVertical(Action<Vector2, Vector2> action, bool isRaw = false)
        {
            var value = isRaw
                            ? _horizontalAndVerticalRawProperty
                            : _horizontalAndVerticalProperty;

            value.Register(action).UnregisterWhenGameObjectDestroyed(Instance);
        }

        public void UnregisterHorizontalAndVertical(Action<Vector2, Vector2> action, bool isRaw = false)
        {
            var value = isRaw
                            ? _horizontalAndVerticalRawProperty
                            : _horizontalAndVerticalProperty;

            value.Unregister(action);
        }

        public void UnregisterHorizontalAndVertical(bool isRaw)
        {
            var value = isRaw
                            ? _horizontalAndVerticalRawProperty
                            : _horizontalAndVerticalProperty;

            value.UnregisterAll();
        }

        public void UnregisterHorizontalAndVertical()
        {
            _horizontalAndVerticalRawProperty.UnregisterAll();
            _horizontalAndVerticalProperty.UnregisterAll();
        }

        public void UnregisterAll()
        {
            foreach (var pair in _axisInputProperties)
            {
                pair.Value.UnregisterAll();
            }

            _axisInputProperties.Clear();
        }

    #endregion
    }
}