// ------------------------------------------------------------
// @file       OnTriggerEnterEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:20
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnTriggerEnterEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerEnterEvent = new();

        private void OnTriggerEnter(Collider collider)
        {
            OnTriggerEnterEvent.Trigger(collider);
        }
    }

    public static class OnTriggerEnterEventTriggerExtension
    {
        public static IUnregister OnTriggerEnterEvent<T>(this T self, Action<Collider> onTriggerEnter, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerEnterEventTrigger>().OnTriggerEnterEvent
               .Register(onTriggerEnter, priority);
        }

        public static IUnregister OnTriggerEnterEvent(this GameObject self, Action<Collider> onTriggerEnter, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerEnterEventTrigger>().OnTriggerEnterEvent
               .Register(onTriggerEnter, priority);
        }
    }
}