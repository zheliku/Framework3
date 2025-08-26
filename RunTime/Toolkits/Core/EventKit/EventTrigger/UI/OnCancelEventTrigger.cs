// ------------------------------------------------------------
// @file       OnCancelEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:49:58
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnCancelEventTrigger : MonoBehaviour, ICancelHandler
    {
        public readonly EasyEvent<BaseEventData> OnCancelEvent = new EasyEvent<BaseEventData>();

        public void OnCancel(BaseEventData eventData)
        {
            OnCancelEvent.Trigger(eventData);
        }
    }

    public static class OnCancelEventTriggerExtension
    {
        public static IUnregister OnCancelEvent<T>(this T self, Action<BaseEventData> onCancel, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnCancelEventTrigger>().OnCancelEvent.Register(onCancel, priority);
        }

        public static IUnregister OnCancelEvent(this GameObject self, Action<BaseEventData> onCancel, float priority = 0)
        {
            return self.GetOrAddComponent<OnCancelEventTrigger>().OnCancelEvent.Register(onCancel, priority);
        }
    }
}