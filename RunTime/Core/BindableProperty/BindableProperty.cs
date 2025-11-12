// ------------------------------------------------------------
// @file       BindableProperty.cs
// @brief
// @author     zheliku
// @Modified   2024-10-05 11:10:03
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     绑定事件监听的 Property
    /// </summary>
    /// <typeparam name="TProperty">被绑定的 Property 类型</typeparam>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class BindableProperty<TProperty> : IBindableProperty<TProperty>
    {
    #region Static

        public static implicit operator TProperty(BindableProperty<TProperty> property)
        {
            return property.Value;
        }

        public static explicit operator BindableProperty<TProperty>(TProperty property)
        {
            return new BindableProperty<TProperty>(property);
        }

        public static bool operator ==(TProperty property, BindableProperty<TProperty> bindableProperty)
        {
            if (bindableProperty is null)
            {
                return property is null;
            }

            return bindableProperty.Equals(property);
        }

        public static bool operator !=(TProperty property, BindableProperty<TProperty> bindableProperty)
        {
            return !(property == bindableProperty);
        }

        public static bool operator ==(BindableProperty<TProperty> bindableProperty, TProperty property)
        {
            return property == bindableProperty;
        }

        public static bool operator !=(BindableProperty<TProperty> bindableProperty, TProperty property)
        {
            return !(bindableProperty == property);
        }

        public static bool operator ==(BindableProperty<TProperty> bindableProperty1, BindableProperty<TProperty> bindableProperty2)
        {
            if (bindableProperty1 is null)
            {
                return bindableProperty2 is null || bindableProperty2.Value is null;
            }

            return bindableProperty1.Equals(bindableProperty2);
        }

        public static bool operator !=(BindableProperty<TProperty> bindableProperty1, BindableProperty<TProperty> bindableProperty2)
        {
            return !(bindableProperty1 == bindableProperty2);
        }

    #endregion

    #region 字段

        protected bool _triggerWhenSameValue;

        protected TProperty _value;

    #if ODIN_INSPECTOR
        [ShowInInspector] [PropertyOrder(1)]
    #endif
        protected EasyEvent<TProperty, TProperty> _onValueChanged = new();

    #endregion

    #region 接口

    #if ODIN_INSPECTOR
        [ShowInInspector] [PropertyOrder(0)]
    #endif
        public TProperty Value
        {
            get => GetValue();
            set
            {
                var oldValue = _value;

                if (value == this)
                {
                    if (_triggerWhenSameValue)
                    {
                        _onValueChanged.Trigger(oldValue, value);
                    }

                    return;
                }

                SetValue(value);

                _onValueChanged.Trigger(oldValue, value);
            }
        }

        public int EventCount
        {
            get => _onValueChanged.EventCount;
        }

        public void SetValueWithoutNotify(TProperty newValue)
        {
            _value = newValue;
        }

        public IUnregister RegisterWithInitValue(Action<TProperty, TProperty> onValueChanged, float priority = 0)
        {
            onValueChanged(_value, _value);
            return Register(onValueChanged, priority);
        }

        public void Unregister(Action<TProperty, TProperty> onValueChanged)
        {
            _onValueChanged.Unregister(onValueChanged);
        }

        public void UnregisterAll() { _onValueChanged.UnregisterAll(); }

        public IUnregister Register(Action<TProperty, TProperty> onValueChanged, float priority = 0)
        {
            return _onValueChanged.Register(onValueChanged, priority);
        }

        // 仅能通过 IEasyEvent 接口使用 Register(Action onEvent) 方法
        IUnregister IEasyEvent.Register(Action onEvent, float priority) { return Register((_, _) => onEvent()); }

    #endregion

    #region 方法

        public BindableProperty(TProperty defaultValue = default, bool triggerWhenSameValue = false)
        {
            _value                = defaultValue;
            _triggerWhenSameValue = triggerWhenSameValue;
        }

        protected virtual TProperty GetValue()
        {
            return _value;
        }

        protected virtual void SetValue(TProperty newValue)
        {
            _value = newValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return Value is null;
            }

            if (obj is TProperty property)
            {
                return Equals(Value, property);
            }

            if (obj is BindableProperty<TProperty> bindableProperty)
            {
                return Equals(Value, (TProperty) bindableProperty);
            }

            return false;
        }

        protected bool Equals(BindableProperty<TProperty> other)
        {
            if (other is null)
            {
                return Value is null;
            }

            return Equals(Value, (TProperty) other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    #endregion
    }
}