﻿using Framework3.Core;

namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Framework3.Core;
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
        public static IUnRegister OnDestroyEvent<T>(this T self, Action onDestroy, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent
                       .Register(onDestroy, priority);
        }

        public static IUnRegister OnDestroyEvent(this GameObject self, Action onDestroy, float priority = 0)
        {
            return self.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent
                       .Register(onDestroy, priority);
        }
    }
}