// ------------------------------------------------------------
// @file       EnumEventSystem.cs
// @brief
// @author     zheliku
// @Modified   2024-10-23 16:10:34
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.EventKit
{
    using System;
    using System.Collections.Generic;
    using Core;
    using FluentAPI;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class EnumEventSystem
    {
        public static readonly EnumEventSystem Global = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly Dictionary<Enum, IEasyEvent> _events = new(50);

        protected EnumEventSystem() { }

        public IUnregister Register<TEnum>(TEnum key, Action<TEnum, object[]> onEvent, float priority = 0) where TEnum : Enum
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TEnum, object[]>>();
                return easyEvent.Register(onEvent, priority);
            }
            else
            {
                var easyEvent = new EasyEvent<TEnum, object[]>();
                _events.Add(key, easyEvent);
                return easyEvent.Register(onEvent, priority);
            }
        }

        public bool Unregister<TEnum>(TEnum key, Action<TEnum, object[]> onEvent) where TEnum : Enum
        {
            if (_events.TryGetValue(key, out var e))
            {
                e.As<EasyEvent<TEnum, object[]>>()?.Unregister(onEvent);
                return true;
            }

            return false;
        }

        public bool Unregister<TEnum>(TEnum key) where TEnum : Enum
        {
            return _events.Remove(key);
        }

        public void UnregisterAll()
        {
            _events.Clear();
        }

        public void Send<TEnum>(TEnum key, params object[] args) where TEnum : Enum
        {
            if (_events.TryGetValue(key, out var e))
            {
                e.As<EasyEvent<TEnum, object[]>>().Trigger(key, args);
            }
        }
    }
}