// ------------------------------------------------------------
// @file       KeyCodeInput.cs
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

    [MonoSingletonPath("Framework3/InputKit/KeyCodeInput")]
    public class KeyCodeInput : MonoSingleton<KeyCodeInput>
    {
    #region Unity 事件

        protected override void Update()
        {
            base.Update();

            if (!OldInputKit.EnableKeyCode)
            {
                return;
            }

            foreach (var pair in _keyCodePressProperties)
            {
                var property = pair.Value;

                property.Value = Input.GetKeyDown(pair.Key);
            }

            foreach (var pair in _keyCodeHoldProperties)
            {
                var property = pair.Value;

                property.Value = Input.GetKey(pair.Key);
            }

            foreach (var pair in _keyCodeReleaseProperties)
            {
                var property = pair.Value;

                property.Value = Input.GetKeyUp(pair.Key);
            }
        }

    #endregion

    #region 其他方法

        private Dictionary<KeyCode, BindableKeyCodeInputProperty> GetMouseDic(InputType inputType)
        {
            return inputType switch
            {
                InputType.Press   => _keyCodePressProperties,
                InputType.Hold    => _keyCodeHoldProperties,
                InputType.Release => _keyCodeReleaseProperties,
                _                 => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
            };
        }

    #endregion

    #region 字段

    #if ODIN_INSPECTOR
        [ShowInInspector]
        [LabelText("KeyCode Press")]
        [DictionaryDrawerSettings(KeyLabel = "KeyCode", ValueLabel = "Value")]
    #endif
        private Dictionary<KeyCode, BindableKeyCodeInputProperty> _keyCodePressProperties = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
        [LabelText("KeyCode Hold")] [PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "KeyCode", ValueLabel = "Value")]
    #endif
        private Dictionary<KeyCode, BindableKeyCodeInputProperty> _keyCodeHoldProperties = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
        [LabelText("KeyCode Release")] [PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "KeyCode", ValueLabel = "Value")]
    #endif
        private Dictionary<KeyCode, BindableKeyCodeInputProperty> _keyCodeReleaseProperties = new();

    #endregion

    #region 公共方法

        public void Register(KeyCode keyCode, Action<bool, bool> action, InputType inputType)
        {
            var dic = GetMouseDic(inputType);

            if (!dic.TryGetValue(keyCode, out var value))
            {
                value = new BindableKeyCodeInputProperty();

                dic.Add(keyCode, value);
            }

            value.Register(action).UnregisterWhenGameObjectDestroyed(Instance);
        }

        public void Unregister(KeyCode keyCode, Action<bool, bool> action, InputType inputType)
        {
            var dic = GetMouseDic(inputType);

            if (dic.TryGetValue(keyCode, out var value))
            {
                value.Unregister(action);

                if (value.EventCount == 0)
                {
                    dic.Remove(keyCode);
                }
            }
        }

        public void Unregister(KeyCode keyCode, InputType inputType)
        {
            var dic = GetMouseDic(inputType);

            if (dic.TryGetValue(keyCode, out var value))
            {
                value.UnregisterAll();
                dic.Remove(keyCode);
            }
        }

        public void Unregister(KeyCode keyCode)
        {
            if (_keyCodePressProperties.TryGetValue(keyCode, out var value))
            {
                value.UnregisterAll();
                _keyCodePressProperties.Remove(keyCode);
            }

            if (_keyCodeHoldProperties.TryGetValue(keyCode, out value))
            {
                value.UnregisterAll();
                _keyCodeHoldProperties.Remove(keyCode);
            }

            if (_keyCodeReleaseProperties.TryGetValue(keyCode, out value))
            {
                value.UnregisterAll();
                _keyCodeReleaseProperties.Remove(keyCode);
            }
        }

        public void UnregisterAll()
        {
            foreach (var pair in _keyCodePressProperties)
            {
                pair.Value.UnregisterAll();
            }

            foreach (var pair in _keyCodeHoldProperties)
            {
                pair.Value.UnregisterAll();
            }

            foreach (var pair in _keyCodeReleaseProperties)
            {
                pair.Value.UnregisterAll();
            }

            _keyCodePressProperties.Clear();
            _keyCodeHoldProperties.Clear();
            _keyCodeReleaseProperties.Clear();
        }

    #endregion
    }
}