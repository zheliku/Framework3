// ------------------------------------------------------------
// @file       OnTriggerStayEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:44
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnTriggerStayEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerStayEvent = new EasyEvent<Collider>();

        private void OnTriggerStay(Collider collider)
        {
            OnTriggerStayEvent.Trigger(collider);
        }
    }

    public static class OnTriggerStayEventTriggerExtension
    {
        public static IUnregister OnTriggerStayEvent<T>(this T self, Action<Collider> onTriggerStay, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerStayEventTrigger>().OnTriggerStayEvent
                       .Register(onTriggerStay, priority);
        }

        public static IUnregister OnTriggerStayEvent(this GameObject self, Action<Collider> onTriggerStay, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerStayEventTrigger>().OnTriggerStayEvent
                       .Register(onTriggerStay, priority);
        }
    }
}