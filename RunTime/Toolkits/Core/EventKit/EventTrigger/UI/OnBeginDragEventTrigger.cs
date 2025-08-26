// ------------------------------------------------------------
// @file       OnBeginDragEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:52
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnBeginDragEventTrigger : MonoBehaviour, IBeginDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnBeginDragEvent = new EasyEvent<PointerEventData>();

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent.Trigger(eventData);
        }
    }

    public static class OnBeginDragEventTriggerExtension
    {
        public static IUnregister OnBeginDragEvent<T>(this T self, Action<PointerEventData> onBeganDrag, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnBeginDragEventTrigger>().OnBeginDragEvent.Register(onBeganDrag, priority);
        }

        public static IUnregister OnBeginDragEvent(this GameObject self, Action<PointerEventData> onBeganDrag, float priority = 0)
        {
            return self.GetOrAddComponent<OnBeginDragEventTrigger>().OnBeginDragEvent.Register(onBeganDrag, priority);
        }
    }
}