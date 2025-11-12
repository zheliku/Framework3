// ------------------------------------------------------------
// @file       OnPointerDownEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:49
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnPointerDownEventTrigger : MonoBehaviour, IPointerDownHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerDownEvent = new();

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent.Trigger(eventData);
        }
    }

    public static class OnPointerDownEventTriggerExtension
    {
        public static IUnregister OnPointerDownEvent<T>(this T self, Action<PointerEventData> onPointerDownEvent, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnPointerDownEventTrigger>().OnPointerDownEvent
               .Register(onPointerDownEvent, priority);
        }

        public static IUnregister OnPointerDownEvent(this GameObject self, Action<PointerEventData> onPointerDownEvent, float priority = 0)
        {
            return self.GetOrAddComponent<OnPointerDownEventTrigger>().OnPointerDownEvent
               .Register(onPointerDownEvent, priority);
        }
    }
}