// ------------------------------------------------------------
// @file       OnDeselectEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:02
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnDeselectEventTrigger : MonoBehaviour, IDeselectHandler
    {
        public readonly EasyEvent<BaseEventData> OnDeselectEvent = new EasyEvent<BaseEventData>();
        
        public void OnDeselect(BaseEventData eventData)
        {
            OnDeselectEvent.Trigger(eventData);
        }
    }

    public static class OnDeselectEventTriggerExtension
    {
        public static IUnregister OnDeselectEvent<T>(this T self, Action<BaseEventData> onDeselect, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnDeselectEventTrigger>().OnDeselectEvent.Register(onDeselect, priority);
        }

        public static IUnregister OnDeselectEvent(this GameObject self, Action<BaseEventData> onDeselect, float priority = 0)
        {
            return self.GetOrAddComponent<OnDeselectEventTrigger>().OnDeselectEvent.Register(onDeselect, priority);
        }
    }
}