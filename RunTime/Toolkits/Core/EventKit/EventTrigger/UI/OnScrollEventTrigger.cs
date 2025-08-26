// ------------------------------------------------------------
// @file       OnScrollEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:51:09
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnScrollEventTrigger : MonoBehaviour, IScrollHandler
    {
        public readonly EasyEvent<PointerEventData> OnScrollEvent = new EasyEvent<PointerEventData>();

        public void OnScroll(PointerEventData eventData)
        {
            OnScrollEvent.Trigger(eventData);
        }
    }

    public static class OnScrollEventTriggerExtension
    {
        public static IUnregister OnScrollEvent<T>(this T self, Action<PointerEventData> onScroll, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnScrollEventTrigger>().OnScrollEvent.Register(onScroll, priority);
        }

        public static IUnregister OnScrollEvent(this GameObject self, Action<PointerEventData> onScroll, float priority = 0)
        {
            return self.GetOrAddComponent<OnScrollEventTrigger>().OnScrollEvent.Register(onScroll, priority);
        }
    }
}