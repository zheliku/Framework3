// ------------------------------------------------------------
// @file       OnCollisionStay2DEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:48:49
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnCollisionStay2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision2D> OnCollisionStay2DEvent = new();

        private void OnCollisionStay2D(Collision2D col)
        {
            OnCollisionStay2DEvent.Trigger(col);
        }
    }

    public static class OnCollisionStay2DEventTriggerExtension
    {
        public static IUnregister OnCollisionStay2DEvent<T>(this T self, Action<Collision2D> onCollisionStay2D, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionStay2DEventTrigger>().OnCollisionStay2DEvent
               .Register(onCollisionStay2D, priority);
        }

        public static IUnregister OnCollisionStay2DEvent(this GameObject self, Action<Collision2D> onCollisionStay2D, float priority = 0)
        {
            return self.GetOrAddComponent<OnCollisionStay2DEventTrigger>().OnCollisionStay2DEvent
               .Register(onCollisionStay2D, priority);
        }
    }
}