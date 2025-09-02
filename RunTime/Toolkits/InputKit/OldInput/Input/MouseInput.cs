// ------------------------------------------------------------
// @file       MouseInput.cs
// @brief
// @author     zheliku
// @Modified   2024-08-23 23:08:33
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System;
    using Core;
    using System.Collections.Generic;
    using SingletonKit;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [MonoSingletonPath("Framework/InputKit/MouseInput")]
    public class MouseInput : MonoSingleton<MouseInput>
    {
    #region 字段

        [ShowInInspector] [LabelText("Mouse Press")] [PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "MouseButton", ValueLabel = "Value")]
        private Dictionary<MouseInputType, BindableMouseInputProperty> _mousePressProperties = new Dictionary<MouseInputType, BindableMouseInputProperty>();

        [ShowInInspector] [LabelText("Mouse Hold")] [PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "MouseButton", ValueLabel = "Value")]
        private Dictionary<MouseInputType, BindableMouseInputProperty> _mouseHoldProperties = new Dictionary<MouseInputType, BindableMouseInputProperty>();

        [ShowInInspector] [LabelText("Mouse Release")] [PropertySpace]
        [DictionaryDrawerSettings(KeyLabel = "MouseButton", ValueLabel = "Value")]
        private Dictionary<MouseInputType, BindableMouseInputProperty> _mouseReleaseProperties = new Dictionary<MouseInputType, BindableMouseInputProperty>();

    #endregion

    #region 属性

    #endregion

    #region 公共方法

        public void Register(MouseInputType mouseType, Action<bool, bool> action, InputType inputType)
        {
            var dic = GetMouseDic(inputType);

            if (!dic.TryGetValue(mouseType, out var value))
            {
                value = new BindableMouseInputProperty();

                dic.Add(mouseType, value);
            }

            value.Register(action).UnregisterWhenGameObjectDestroyed(Instance);
        }
        
        public void Unregister(MouseInputType mouseType, Action<bool, bool> action, InputType inputType)
        {
            var dic = GetMouseDic(inputType);

            if (dic.TryGetValue(mouseType, out var value))
            {
                value.Unregister(action);
                
                if (value.EventCount == 0)
                {
                    dic.Remove(mouseType);
                }
            }
        }
        
        public void Unregister(MouseInputType mouseType, InputType inputType)
        {
            var dic = GetMouseDic(inputType);

            if (dic.TryGetValue(mouseType, out var value))
            {
                value.UnregisterAll();
                dic.Remove(mouseType);
            }
        }
        
        public void Unregister(MouseInputType mouseType)
        {
            if (_mousePressProperties.TryGetValue(mouseType, out var value))
            {
                value.UnregisterAll();
                _mousePressProperties.Remove(mouseType);
            }
            
            if (_mouseHoldProperties.TryGetValue(mouseType, out value))
            {
                value.UnregisterAll();
                _mouseHoldProperties.Remove(mouseType);
            }
            
            if (_mouseReleaseProperties.TryGetValue(mouseType, out value))
            {
                value.UnregisterAll();
                _mouseReleaseProperties.Remove(mouseType);
            }
        }

        public void UnregisterAll()
        {
            foreach (var pair in _mousePressProperties)
            {
                pair.Value.UnregisterAll();
            }
            
            foreach (var pair in _mouseHoldProperties)
            {
                pair.Value.UnregisterAll();
            }
            
            foreach (var pair in _mouseReleaseProperties)
            {
                pair.Value.UnregisterAll();
            }
            
            _mousePressProperties.Clear();
            _mouseHoldProperties.Clear();
            _mouseReleaseProperties.Clear();
        }

    #endregion

    #region 其他方法

        private Dictionary<MouseInputType, BindableMouseInputProperty> GetMouseDic(InputType inputType)
        {
            return inputType switch
            {
                InputType.Press   => _mousePressProperties,
                InputType.Hold    => _mouseHoldProperties,
                InputType.Release => _mouseReleaseProperties,
                _                 => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
            };
        }

    #endregion

    #region Unity 事件

        protected override void Update()
        {
            base.Update();

            if (!OldInputKit.EnableMouse)
            {
                return;
            }
            
            foreach (var pair in _mousePressProperties)
            {
                var property = pair.Value;

                property.Value = Input.GetMouseButtonDown((int) pair.Key);
            }
            
            foreach (var pair in _mouseHoldProperties)
            {
                var property = pair.Value;

                property.Value = Input.GetMouseButton((int) pair.Key);
            }
            
            foreach (var pair in _mouseReleaseProperties)
            {
                var property = pair.Value;

                property.Value = Input.GetMouseButtonUp((int) pair.Key);
            }
        }
        

    #endregion
    }
}