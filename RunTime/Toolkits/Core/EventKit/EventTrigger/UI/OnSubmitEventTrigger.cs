// ------------------------------------------------------------
// @file       OnSubmitEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:51:19
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnSubmitEventTrigger : MonoBehaviour, ISubmitHandler
    {
        public readonly EasyEvent<BaseEventData> OnSubmitEvent = new EasyEvent<BaseEventData>();

        public void OnSubmit(BaseEventData eventData)
        {
            OnSubmitEvent.Trigger(eventData);
        }
    }

    public static class OnSubmitEventTriggerExtension
    {
        public static IUnRegister OnSubmitEvent<T>(this T self, Action<BaseEventData> onSubmit, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnSubmitEventTrigger>().OnSubmitEvent.Register(onSubmit, priority);
        }

        public static IUnRegister OnSubmitEvent(this GameObject self, Action<BaseEventData> onSubmit, float priority = 0)
        {
            return self.GetOrAddComponent<OnSubmitEventTrigger>().OnSubmitEvent.Register(onSubmit, priority);
        }
    }
}