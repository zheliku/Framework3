// ------------------------------------------------------------
// @file       StringEventSystem.cs
// @brief
// @author     zheliku
// @Modified   2024-10-23 15:10:42
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

    public class StringEventSystem
    {
        public static readonly StringEventSystem Global = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly Dictionary<string, IEasyEvent> _events = new(50);

        public IUnregister Register(string key, Action onEvent, float priority = 0)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                return easyEvent.Register(onEvent, priority);
            }
            else
            {
                var easyEvent = new EasyEvent();
                _events.Add(key, easyEvent);
                return easyEvent.Register(onEvent, priority);
            }
        }

        public IUnregister Register<TArg>(string key, Action<TArg> onEvent, float priority = 0)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg>>();
                return easyEvent.Register(onEvent, priority);
            }
            else
            {
                var easyEvent = new EasyEvent<TArg>();
                _events.Add(key, easyEvent);
                return easyEvent.Register(onEvent, priority);
            }
        }

        public IUnregister Register<TArg1, TArg2>(string key, Action<TArg1, TArg2> onEvent, float priority = 0)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg1, TArg2>>();
                return easyEvent.Register(onEvent, priority);
            }
            else
            {
                var easyEvent = new EasyEvent<TArg1, TArg2>();
                _events.Add(key, easyEvent);
                return easyEvent.Register(onEvent, priority);
            }
        }

        public IUnregister Register<TArg1, TArg2, TArg3>(string key, Action<TArg1, TArg2, TArg3> onEvent, float priority = 0)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg1, TArg2, TArg3>>();
                return easyEvent.Register(onEvent, priority);
            }
            else
            {
                var easyEvent = new EasyEvent<TArg1, TArg2, TArg3>();
                _events.Add(key, easyEvent);
                return easyEvent.Register(onEvent, priority);
            }
        }

        public bool Unregister(string key, Action onEvent)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                easyEvent?.Unregister(onEvent);
                return true;
            }

            return false;
        }

        public bool Unregister<TArg>(string key, Action<TArg> onEvent)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg>>();
                easyEvent?.Unregister(onEvent);
                return true;
            }

            return false;
        }

        public bool Unregister<TArg1, TArg2>(string key, Action<TArg1, TArg2> onEvent)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg1, TArg2>>();
                easyEvent?.Unregister(onEvent);
                return true;
            }

            return false;
        }

        public bool Unregister<TArg1, TArg2, TArg3>(string key, Action<TArg1, TArg2, TArg3> onEvent)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg1, TArg2, TArg3>>();
                easyEvent?.Unregister(onEvent);
                return true;
            }

            return false;
        }

        public bool Unregister(string key)
        {
            return _events.Remove(key);
        }

        public void UnregisterAll()
        {
            _events.Clear();
        }

        public void Send(string key)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                easyEvent?.Trigger();
            }
        }

        public void Send<TArg>(string key, TArg data)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg>>();
                easyEvent?.Trigger(data);
            }
        }

        public void Send<TArg1, TArg2>(string key, TArg1 arg1, TArg2 arg2)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg1, TArg2>>();
                easyEvent?.Trigger(arg1, arg2);
            }
        }

        public void Send<TArg1, TArg2, TArg3>(string key, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            if (_events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent<TArg1, TArg2, TArg3>>();
                easyEvent?.Trigger(arg1, arg2, arg3);
            }
        }
    }
}