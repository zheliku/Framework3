// ------------------------------------------------------------
// @file       OnDestroyEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:37:08
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;

    public class OnDestroyEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnDestroyEvent = new EasyEvent();

        private void OnDestroy()
        {
            OnDestroyEvent.Trigger();
        }
    }

    public static class OnDestroyEventTriggerExtension
    {
        public static IUnregister OnDestroyEvent<T>(this T self, Action onDestroy, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent
                       .Register(onDestroy, priority);
        }

        public static IUnregister OnDestroyEvent(this GameObject self, Action onDestroy, float priority = 0)
        {
            return self.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent
                       .Register(onDestroy, priority);
        }
    }
}