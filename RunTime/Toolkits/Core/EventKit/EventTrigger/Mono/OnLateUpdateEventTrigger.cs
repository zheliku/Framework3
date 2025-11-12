// ------------------------------------------------------------
// @file       OnLateUpdateEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:44
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnLateUpdateEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent LateUpdateEvent = new();

        private void LateUpdate()
        {
            LateUpdateEvent.Trigger();
        }
    }

    public static class OnLateUpdateEventTriggerExtension
    {
        public static IUnregister OnLateUpdateEvent<T>(this T self, Action update, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnLateUpdateEventTrigger>().LateUpdateEvent
               .Register(update, priority);
        }

        public static IUnregister OnLateUpdateEvent(this GameObject self, Action update, int priority = 0)
        {
            return self.GetOrAddComponent<OnLateUpdateEventTrigger>().LateUpdateEvent
               .Register(update, priority);
        }
    }
}