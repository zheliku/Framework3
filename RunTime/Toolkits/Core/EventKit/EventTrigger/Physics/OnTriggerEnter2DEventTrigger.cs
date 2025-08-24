// ------------------------------------------------------------
// @file       OnTriggerEnter2DEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:00
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnTriggerEnter2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerEnter2DEvent = new EasyEvent<Collider2D>();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnTriggerEnter2DEvent.Trigger(collider);
        }
    }

    public static class OnTriggerEnter2DEventTriggerExtension
    {
        public static IUnRegister OnTriggerEnter2DEvent<T>(this T self, Action<Collider2D> onTriggerEnter2D, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerEnter2DEventTrigger>().OnTriggerEnter2DEvent
                       .Register(onTriggerEnter2D, priority);
        }

        public static IUnRegister OnTriggerEnter2DEvent(this GameObject self, Action<Collider2D> onTriggerEnter2D, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerEnter2DEventTrigger>().OnTriggerEnter2DEvent
                       .Register(onTriggerEnter2D, priority);
        }
    }
}