// ------------------------------------------------------------
// @file       OnFixedUpdateEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:40
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnFixedUpdateEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent FixedUpdateEvent = new EasyEvent();

        private void FixedUpdate()
        {
            FixedUpdateEvent.Trigger();
        }
    }

    public static class OnFixedUpdateEventTriggerExtension
    {
        public static IUnRegister OnFixedUpdateEvent<T>(this T self, Action update, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnFixedUpdateEventTrigger>().FixedUpdateEvent
                       .Register(update, priority);
        }

        public static IUnRegister OnFixedUpdateEvent(this GameObject self, Action update, int priority = 0)
        {
            return self.GetOrAddComponent<OnFixedUpdateEventTrigger>().FixedUpdateEvent
                       .Register(update, priority);
        }
    }
}