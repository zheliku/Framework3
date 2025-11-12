// ------------------------------------------------------------
// @file       OnBecomeInvisibleEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:02
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnBecomeInvisibleEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnBecameInvisibleEvent = new();

        private void OnBecameInvisible()
        {
            OnBecameInvisibleEvent.Trigger();
        }
    }

    public static class OnBecameInvisibleEventTriggerExtension
    {
        public static IUnregister OnBecameInvisibleEvent<T>(this T self, Action onBecameInvisible, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnBecomeInvisibleEventTrigger>().OnBecameInvisibleEvent
               .Register(onBecameInvisible, priority);
        }

        public static IUnregister OnBecameInvisibleEvent(this GameObject self, Action onBecameInvisible, float priority = 0)
        {
            return self.GetOrAddComponent<OnBecomeInvisibleEventTrigger>().OnBecameInvisibleEvent
               .Register(onBecameInvisible, priority);
        }
    }
}