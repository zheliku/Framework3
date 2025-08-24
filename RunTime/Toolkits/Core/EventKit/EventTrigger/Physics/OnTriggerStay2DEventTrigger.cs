// ------------------------------------------------------------
// @file       OnTriggerStay2DEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:38
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnTriggerStay2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerStay2DEvent = new EasyEvent<Collider2D>();

        private void OnTriggerStay2D(Collider2D collider)
        {
            OnTriggerStay2DEvent.Trigger(collider);
        }
    }

    public static class OnTriggerStay2DEventTriggerExtension
    {
        public static IUnRegister OnTriggerStay2DEvent<T>(this T self, Action<Collider2D> onTriggerStay2D, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerStay2DEventTrigger>().OnTriggerStay2DEvent
                       .Register(onTriggerStay2D, priority);
        }

        public static IUnRegister OnTriggerStay2DEvent(this GameObject self, Action<Collider2D> onTriggerStay2D, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerStay2DEventTrigger>().OnTriggerStay2DEvent
                       .Register(onTriggerStay2D, priority);
        }
    }
}