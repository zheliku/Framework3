// ------------------------------------------------------------
// @file       OnUpdateSelectedEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:51:25
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnUpdateSelectedEventTrigger : MonoBehaviour, IUpdateSelectedHandler
    {
        public readonly EasyEvent<BaseEventData> OnUpdateSelectedEvent = new();


        public void OnUpdateSelected(BaseEventData eventData)
        {
            OnUpdateSelectedEvent.Trigger(eventData);
        }
    }

    public static class OnUpdateSelectedEventTriggerExtension
    {
        public static IUnregister OnUpdateSelectedEvent<T>(this T self, Action<BaseEventData> onUpdateSelected, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnUpdateSelectedEventTrigger>().OnUpdateSelectedEvent.Register(onUpdateSelected, priority);
        }

        public static IUnregister OnUpdateSelectedEvent(this GameObject self, Action<BaseEventData> onUpdateSelected, float priority = 0)
        {
            return self.GetOrAddComponent<OnUpdateSelectedEventTrigger>().OnUpdateSelectedEvent.Register(onUpdateSelected, priority);
        }
    }
}