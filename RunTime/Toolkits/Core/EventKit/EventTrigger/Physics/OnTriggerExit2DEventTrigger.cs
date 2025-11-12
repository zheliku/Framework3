// ------------------------------------------------------------
// @file       OnTriggerExit2DEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:25
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnTriggerExit2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerExit2DEvent = new();

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnTriggerExit2DEvent.Trigger(collider);
        }
    }

    public static class OnTriggerExit2DEventTriggerExtension
    {
        public static IUnregister OnTriggerExit2DEvent<T>(this T self, Action<Collider2D> onTriggerExit2D, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerExit2DEventTrigger>().OnTriggerExit2DEvent
               .Register(onTriggerExit2D, priority);
        }

        public static IUnregister OnTriggerExit2DEvent(this GameObject self, Action<Collider2D> onTriggerExit2D, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerExit2DEventTrigger>().OnTriggerExit2DEvent
               .Register(onTriggerExit2D, priority);
        }
    }
}