// ------------------------------------------------------------
// @file       OnPointerExitEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:59
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnPointerExitEventTrigger : MonoBehaviour, IPointerExitHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerExitEvent = new EasyEvent<PointerEventData>();

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent.Trigger(eventData);
        }
    }

    public static class OnPointerExitEventTriggerExtension
    {
        public static IUnRegister OnPointerExitEvent<T>(this T self, Action<PointerEventData> onPointerExit, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnPointerExitEventTrigger>().OnPointerExitEvent.Register(onPointerExit, priority);
        }

        public static IUnRegister OnPointerExitEvent(this GameObject self, Action<PointerEventData> onPointerExit, float priority = 0)
        {
            return self.GetOrAddComponent<OnPointerExitEventTrigger>().OnPointerExitEvent.Register(onPointerExit, priority);
        }
    }
}