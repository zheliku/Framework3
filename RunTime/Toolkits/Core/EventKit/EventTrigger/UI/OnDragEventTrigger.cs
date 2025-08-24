// ------------------------------------------------------------
// @file       OnDragEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:17
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnDragEventTrigger : MonoBehaviour, IDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnDragEvent = new EasyEvent<PointerEventData>();

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent.Trigger(eventData);
        }
    }

    public static class OnDragEventTriggerExtension
    {
        public static IUnRegister OnDragEvent<T>(this T self, Action<PointerEventData> onDrag, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnDragEventTrigger>().OnDragEvent.Register(onDrag, priority);
        }

        public static IUnRegister OnDragEvent(this GameObject self, Action<PointerEventData> onDrag, float priority = 0)
        {
            return self.GetOrAddComponent<OnDragEventTrigger>().OnDragEvent.Register(onDrag, priority);
        }
    }
}