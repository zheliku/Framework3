// ------------------------------------------------------------
// @file       OnCollisionExitEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:48:40
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnCollisionExitEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision> OnCollisionExitEvent = new();

        private void OnCollisionExit(Collision col)
        {
            OnCollisionExitEvent.Trigger(col);
        }
    }

    public static class OnCollisionExitEventTriggerExtension
    {
        public static IUnregister OnCollisionExitEvent<T>(this T self, Action<Collision> onCollisionExit, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionExitEventTrigger>().OnCollisionExitEvent
               .Register(onCollisionExit, priority);
        }

        public static IUnregister OnCollisionExitEvent(this GameObject self, Action<Collision> onCollisionExit, float priority = 0)
        {
            return self.GetOrAddComponent<OnCollisionExitEventTrigger>().OnCollisionExitEvent
               .Register(onCollisionExit, priority);
        }
    }
}