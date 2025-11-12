// ------------------------------------------------------------
// @file       OnPointerUpEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:51:04
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnPointerUpEventTrigger : MonoBehaviour, IPointerUpHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerUpEvent = new();

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent.Trigger(eventData);
        }
    }

    public static class OnPointerUpEventTriggerExtension
    {
        public static IUnregister OnPointerUpEvent<T>(this T self, Action<PointerEventData> onPointerUpEvent, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnPointerUpEventTrigger>().OnPointerUpEvent
               .Register(onPointerUpEvent, priority);
        }

        public static IUnregister OnPointerUpEvent(this GameObject self, Action<PointerEventData> onPointerUpEvent, float priority = 0)
        {
            return self.GetOrAddComponent<OnPointerUpEventTrigger>().OnPointerUpEvent
               .Register(onPointerUpEvent, priority);
        }
    }
}