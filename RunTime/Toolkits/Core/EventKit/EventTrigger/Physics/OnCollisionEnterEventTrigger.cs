// ------------------------------------------------------------
// @file       OnCollisionEnterEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:47:05
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnCollisionEnterEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision> OnCollisionEnterEvent = new EasyEvent<Collision>();

        private void OnCollisionEnter(Collision col)
        {
            OnCollisionEnterEvent.Trigger(col);
        }
    }

    public static class OnCollisionEnterEventTriggerExtension
    {
        public static IUnregister OnCollisionEnterEvent<T>(this T self, Action<Collision> onCollisionEnter, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionEnterEventTrigger>().OnCollisionEnterEvent
                       .Register(onCollisionEnter, priority);
        }

        public static IUnregister OnCollisionEnterEvent(this GameObject self, Action<Collision> onCollisionEnter, float priority = 0)
        {
            return self.GetOrAddComponent<OnCollisionEnterEventTrigger>().OnCollisionEnterEvent
                       .Register(onCollisionEnter, priority);
        }
    }
}