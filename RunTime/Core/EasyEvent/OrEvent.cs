// ------------------------------------------------------------
// @file       OrEvent.cs
// @brief
// @author     zheliku
// @Modified   2024-10-05 11:10:01
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     级联事件
    /// </summary>
    public sealed class OrEvent : IUnregisterList
    {
        private Action            _onEvent = () => { };            // OrEvent 事件
        public  List<IUnregister> UnregisterList { get; } = new(); // 待注销列表

        /// <summary>
        ///     绑定 EasyEvent
        /// </summary>
        /// <param name="easyEvent">IEasyEvent 实例</param>
        /// <returns>OrEvent 自身</returns>
        public OrEvent Or(IEasyEvent easyEvent)
        {
            easyEvent.Register(Trigger)    // 给 easyEvent 绑定 OrEvent 自己的事件
               .AddToUnregisterList(this); // 登记注销
            return this;
        }

        /// <summary>
        ///     注册事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        /// <returns>注销器</returns>
        public IUnregister Register(Action onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnregister(() => { Unregister(onEvent); });
        }

        /// <summary>
        ///     注册并触发一次事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        /// <returns>注销器</returns>
        public IUnregister RegisterWithTrigger(Action onEvent)
        {
            onEvent?.Invoke();
            return Register(onEvent);
        }

        /// <summary>
        ///     注销事件
        /// </summary>
        /// <param name="onEvent">事件</param>
        public void Unregister(Action onEvent)
        {
            _onEvent -= onEvent;
            this.UnregisterAll();
        }

        private void Trigger()
        {
            _onEvent?.Invoke();
        }
    }
}