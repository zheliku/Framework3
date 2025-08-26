// ------------------------------------------------------------
// @file       OnTriggerExitEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:31
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnTriggerExitEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerExitEvent = new EasyEvent<Collider>();

        private void OnTriggerExit(Collider collider)
        {
            OnTriggerExitEvent.Trigger(collider);
        }
    }

    public static class OnTriggerExitEventTriggerExtension
    {
        public static IUnregister OnTriggerExitEvent<T>(this T self, Action<Collider> onTriggerExit, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerExitEventTrigger>().OnTriggerExitEvent
                       .Register(onTriggerExit, priority);
        }

        public static IUnregister OnTriggerExitEvent(this GameObject self, Action<Collider> onTriggerExit, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerExitEventTrigger>().OnTriggerExitEvent
                       .Register(onTriggerExit, priority);
        }
    }
}