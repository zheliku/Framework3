// ------------------------------------------------------------
// @file       OnDisableEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:22
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnDisableEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnDisableEvent = new EasyEvent();

        private void OnDisable()
        {
            OnDisableEvent.Trigger();
        }
    }

    public static class OnDisableEventTriggerExtension
    {
        public static IUnregister OnDisableEvent<T>(this T self, Action onDisable, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent
                       .Register(onDisable, priority);
        }

        public static IUnregister OnDisableEvent(this GameObject self, Action onDisable, float priority = 0)
        {
            return self.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent
                       .Register(onDisable, priority);
        }
    }
}