﻿using Framework3.Core;

namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Framework3.Core;
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
        public static IUnRegister OnBecameVisibleEvent<T>(this T self, Action onBecameVisible, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnBecomeVisibleEventTrigger>().OnBecameVisibleEvent
                       .Register(onBecameVisible, priority);
        }

        public static IUnRegister OnBecameVisibleEvent(this GameObject self, Action onBecameVisible, float priority = 0)
        {
            return self.GetOrAddComponent<OnBecomeVisibleEventTrigger>().OnBecameVisibleEvent
                       .Register(onBecameVisible, priority);
        }
    }
}