// ------------------------------------------------------------
// @file       OnUpdateEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:55
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnUpdateEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent UpdateEvent = new EasyEvent();

        private void Update()
        {
            UpdateEvent.Trigger();
        }
    }

    public static class OnUpdateEventTriggerExtension
    {
        public static IUnregister OnUpdateEvent<T>(this T self, Action update, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnUpdateEventTrigger>().UpdateEvent
                       .Register(update, priority);
        }

        public static IUnregister OnUpdateEvent(this GameObject self, Action update, float priority = 0)
        {
            return self.GetOrAddComponent<OnUpdateEventTrigger>().UpdateEvent
                       .Register(update, priority);
        }
    }
}