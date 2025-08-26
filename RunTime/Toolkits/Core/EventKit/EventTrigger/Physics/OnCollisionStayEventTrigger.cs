// ------------------------------------------------------------
// @file       OnCollisionStayEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:48:55
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnCollisionStayEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision> OnCollisionStayEvent = new EasyEvent<Collision>();

        private void OnCollisionStay(Collision col)
        {
            OnCollisionStayEvent.Trigger(col);
        }
    }

    public static class OnCollisionStayEventTriggerExtension
    {
        public static IUnregister OnCollisionStayEvent<T>(this T self, Action<Collision> onCollisionStay, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionStayEventTrigger>().OnCollisionStayEvent
                       .Register(onCollisionStay, priority);
        }

        public static IUnregister OnCollisionStayEvent(this GameObject self, Action<Collision> onCollisionStay, int priority = 0)
        {
            return self.GetOrAddComponent<OnCollisionStayEventTrigger>().OnCollisionStayEvent
                       .Register(onCollisionStay, priority);
        }
    }
}