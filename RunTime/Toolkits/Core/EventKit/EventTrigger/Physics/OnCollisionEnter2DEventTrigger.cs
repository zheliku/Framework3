// ------------------------------------------------------------
// @file       OnCollisionEnter2DEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:46:58
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnCollisionEnter2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision2D> OnCollisionEnter2DEvent = new EasyEvent<Collision2D>();

        private void OnCollisionEnter2D(Collision2D col)
        {
            OnCollisionEnter2DEvent.Trigger(col);
        }
    }

    public static class OnCollisionEnter2DEventTriggerExtension
    {
        public static IUnregister OnCollisionEnter2DEvent<T>(this T self, Action<Collision2D> onCollisionEnter2D, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionEnter2DEventTrigger>().OnCollisionEnter2DEvent
                       .Register(onCollisionEnter2D, priority);
        }

        public static IUnregister OnCollisionEnter2DEvent(this GameObject self, Action<Collision2D> onCollisionEnter2D, int priority = 0)
        {
            return self.GetOrAddComponent<OnCollisionEnter2DEventTrigger>().OnCollisionEnter2DEvent
                       .Register(onCollisionEnter2D, priority);
        }
    }
}