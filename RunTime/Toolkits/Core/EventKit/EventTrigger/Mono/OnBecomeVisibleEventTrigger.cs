// ------------------------------------------------------------
// @file       OnBecomeVisibleEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:36:56
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnBecomeVisibleEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnBecameVisibleEvent = new EasyEvent();

        private void OnBecameVisible()
        {
            OnBecameVisibleEvent.Trigger();
        }
    }

    public static class OnBecameVisibleEventTriggerExtension
    {
        public static IUnregister OnBecameVisibleEvent<T>(this T self, Action onBecameVisible, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnBecomeVisibleEventTrigger>().OnBecameVisibleEvent
                       .Register(onBecameVisible, priority);
        }

        public static IUnregister OnBecameVisibleEvent(this GameObject self, Action onBecameVisible, float priority = 0)
        {
            return self.GetOrAddComponent<OnBecomeVisibleEventTrigger>().OnBecameVisibleEvent
                       .Register(onBecameVisible, priority);
        }
    }
}