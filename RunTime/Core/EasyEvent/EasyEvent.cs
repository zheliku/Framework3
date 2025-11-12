// ------------------------------------------------------------
// @file       EasyEvent.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 17:10:19
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class EasyEvent : IEasyEvent
    {
        // 定义一个 Action 类型的私有变量，初始值为空
    #if ODIN_INSPECTOR
        [ShowInInspector] [HideLabel]
    #endif
        private PrioritySortedList<Action, float> _onEvent = new();

        public int EventCount { get => _onEvent.Count; }

        // 注册事件，返回 IUnregister 接口
        public IUnregister Register(Action onEvent, float priority = 0)
        {
            _onEvent.Add(onEvent, priority);
            return new CustomUnregister(() => { Unregister(onEvent); }); // 返回自定义 Unregister 接口，用于注销事件，lambda 表达式使用了闭包
        }

        // 注销所有事件
        public void UnregisterAll()
        {
            _onEvent.Clear();
        }

        // 注册并调用事件
        public IUnregister RegisterWithTrigger(Action onEvent, float priority = 0)
        {
            onEvent?.Invoke();
            return Register(onEvent, priority);
        }

        // 注销事件
        public void Unregister(Action onEvent)
        {
            _onEvent.Remove(onEvent);
        }

        // 触发事件
        public void Trigger()
        {
            // 使用 for 循环，防止列表更改导致的报错
            for (var i = 0; i < _onEvent.Count; i++)
            {
                var action = _onEvent[i];
                action.Invoke();
            }
        }
    }

#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class EasyEvent<TArg> : IEasyEvent
    {
    #if ODIN_INSPECTOR
        [ShowInInspector] [HideLabel]
    #endif
        private PrioritySortedList<Action<TArg>, float> _onEvent = new();

        public int EventCount { get => _onEvent.Count; }

        // 注销所有事件
        public void UnregisterAll()
        {
            _onEvent.Clear();
        }

        // 仅能通过 IEasyEvent 接口使用 Register(Action onEvent) 方法
        IUnregister IEasyEvent.Register(Action onEvent, float priority)
        {
            return Register(_ => onEvent(), priority);
        }

        public IUnregister Register(Action<TArg> onEvent, float priority = 0)
        {
            _onEvent.Add(onEvent, priority);
            return new CustomUnregister(() => { Unregister(onEvent); });
        }

        public IUnregister RegisterWithTrigger(Action<TArg> onEvent, TArg t, float priority = 0)
        {
            onEvent?.Invoke(t);
            return Register(onEvent, priority);
        }

        public void Unregister(Action<TArg> onEvent)
        {
            _onEvent.Remove(onEvent);
        }

        public void Trigger(TArg t)
        {
            // 使用 for 循环，防止列表更改导致的报错
            for (var i = 0; i < _onEvent.Count; i++)
            {
                var action = _onEvent[i];
                action.Invoke(t);
            }
        }
    }

#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class EasyEvent<TArg1, TArg2> : IEasyEvent
    {
    #if ODIN_INSPECTOR
        [ShowInInspector] [HideLabel]
    #endif
        private PrioritySortedList<Action<TArg1, TArg2>, float> _onEvent = new();

        public int EventCount { get => _onEvent.Count; }

        // 注销所有事件
        public void UnregisterAll()
        {
            _onEvent.Clear();
        }

        // 仅能通过 IEasyEvent 接口使用 Register(Action onEvent) 方法
        IUnregister IEasyEvent.Register(Action onEvent, float priority)
        {
            return Register((_, _) => onEvent(), priority);
        }

        public IUnregister Register(Action<TArg1, TArg2> onEvent, float priority = 0)
        {
            _onEvent.Add(onEvent, priority);
            return new CustomUnregister(() => { Unregister(onEvent); });
        }

        public IUnregister RegisterWithTrigger(Action<TArg1, TArg2> onEvent, TArg1 t1, TArg2 t2, float priority = 0)
        {
            onEvent?.Invoke(t1, t2);
            return Register(onEvent, priority);
        }

        public void Unregister(Action<TArg1, TArg2> onEvent)
        {
            _onEvent.Remove(onEvent);
        }

        public void Trigger(TArg1 t1, TArg2 t2)
        {
            // 使用 for 循环，防止列表更改导致的报错
            for (var i = 0; i < _onEvent.Count; i++)
            {
                var action = _onEvent[i];
                action.Invoke(t1, t2);
            }
        }
    }

#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class EasyEvent<TArg1, TArg2, TArg3> : IEasyEvent
    {
    #if ODIN_INSPECTOR
        [ShowInInspector] [HideLabel]
    #endif
        private PrioritySortedList<Action<TArg1, TArg2, TArg3>, float> _onEvent = new();

        public int EventCount { get => _onEvent.Count; }

        // 注销所有事件
        public void UnregisterAll()
        {
            _onEvent.Clear();
        }

        // 仅能通过 IEasyEvent 接口使用 Register(Action onEvent) 方法
        IUnregister IEasyEvent.Register(Action onEvent, float priority)
        {
            return Register((_, _, _) => onEvent(), priority);
        }

        public IUnregister Register(Action<TArg1, TArg2, TArg3> onEvent, float priority = 0)
        {
            _onEvent.Add(onEvent, priority);
            return new CustomUnregister(() => { Unregister(onEvent); });
        }

        public IUnregister RegisterWithTrigger(Action<TArg1, TArg2, TArg3> onEvent, TArg1 t1, TArg2 t2, TArg3 t3, float priority = 0)
        {
            onEvent?.Invoke(t1, t2, t3);
            return Register(onEvent, priority);
        }

        public void Unregister(Action<TArg1, TArg2, TArg3> onEvent)
        {
            _onEvent.Remove(onEvent);
        }

        public void Trigger(TArg1 t1, TArg2 t2, TArg3 t3)
        {
            // 使用 for 循环，防止列表更改导致的报错
            for (var i = 0; i < _onEvent.Count; i++)
            {
                var action = _onEvent[i];
                action.Invoke(t1, t2, t3);
            }
        }
    }
}