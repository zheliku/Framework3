// ------------------------------------------------------------
// @file       OnPointerClickEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:43
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnPointerClickEventTrigger : MonoBehaviour, IPointerClickHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerClickEvent = new EasyEvent<PointerEventData>();

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickEvent.Trigger(eventData);
        }
    }

    public static class OnPointerClickEventTriggerExtension
    {
        public static IUnRegister OnPointerClickEvent<T>(this T self, Action<PointerEventData> onPointerClick, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnPointerClickEventTrigger>().OnPointerClickEvent.Register(onPointerClick, priority);
        }

        public static IUnRegister OnPointerClickEvent(this GameObject self, Action<PointerEventData> onPointerClick, float priority = 0)
        {
            return self.GetOrAddComponent<OnPointerClickEventTrigger>().OnPointerClickEvent.Register(onPointerClick, priority);
        }
    }
}