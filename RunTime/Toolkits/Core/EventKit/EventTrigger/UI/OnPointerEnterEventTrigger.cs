// ------------------------------------------------------------
// @file       OnPointerEnterEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:54
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnPointerEnterEventTrigger : MonoBehaviour, IPointerEnterHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerEnterEvent = new();

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent.Trigger(eventData);
        }
    }

    public static class OnPointerEnterEventTriggerExtension
    {
        public static IUnregister OnPointerEnterEvent<T>(this T self, Action<PointerEventData> onPointerEnter, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnPointerEnterEventTrigger>().OnPointerEnterEvent.Register(onPointerEnter, priority);
        }

        public static IUnregister OnPointerEnterEvent(this GameObject self, Action<PointerEventData> onPointerEnter, float priority = 0)
        {
            return self.GetOrAddComponent<OnPointerEnterEventTrigger>().OnPointerEnterEvent.Register(onPointerEnter, priority);
        }
    }
}