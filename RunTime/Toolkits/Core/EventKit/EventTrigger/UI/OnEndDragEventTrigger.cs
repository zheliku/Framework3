// ------------------------------------------------------------
// @file       OnEndDragEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:28
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnEndDragEventTrigger : MonoBehaviour, IEndDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnEndDragEvent = new EasyEvent<PointerEventData>();

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent.Trigger(eventData);
        }
    }

    public static class OnEndDragEventTriggerExtension
    {
        public static IUnRegister OnEndDragEvent<T>(this T self, Action<PointerEventData> onEndDrag, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnEndDragEventTrigger>().OnEndDragEvent.Register(onEndDrag, priority);
        }

        public static IUnRegister OnEndDragEvent(this GameObject self, Action<PointerEventData> onEndDrag, float priority = 0)
        {
            return self.GetOrAddComponent<OnEndDragEventTrigger>().OnEndDragEvent.Register(onEndDrag, priority);
        }
    }
}