// ------------------------------------------------------------
// @file       OnEnableEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:29
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnEnableEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnEnableEvent = new EasyEvent();

        private void OnEnable()
        {
            OnEnableEvent.Trigger();
        }
    }

    public static class OnEnableEventTriggerExtension
    {
        public static IUnregister OnEnableEvent<T>(this T self, Action onEnable, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnEnableEventTrigger>().OnEnableEvent
                       .Register(onEnable, priority);
        }

        public static IUnregister OnEnableEvent(this GameObject self, Action onEnable, float priority = 0)
        {
            return self.GetOrAddComponent<OnEnableEventTrigger>().OnEnableEvent
                       .Register(onEnable, priority);
        }
    }
}