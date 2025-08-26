// ------------------------------------------------------------
// @file       OnSelectEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:51:14
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnSelectEventTrigger : MonoBehaviour, ISelectHandler
    {
        public readonly EasyEvent<BaseEventData> OnSelectEvent = new EasyEvent<BaseEventData>();

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectEvent.Trigger(eventData);
        }
    }

    public static class OnSelectEventTriggerTriggerExtension
    {
        public static IUnregister OnSelectEvent<T>(this T self, Action<BaseEventData> onSelect, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnSelectEventTrigger>().OnSelectEvent.Register(onSelect, priority);
        }

        public static IUnregister OnSelectEvent(this GameObject self, Action<BaseEventData> onSelect, float priority = 0)
        {
            return self.GetOrAddComponent<OnSelectEventTrigger>().OnSelectEvent.Register(onSelect, priority);
        }
    }
}