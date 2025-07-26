using Framework3.Core;

namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Framework3.Core;
    using UnityEngine;

    public class OnEnableEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnEnableEvent = new EasyEvent();

        private void OnEnable()
        {
            OnEnableEvent.Trigger();
        }
    }

    public static class OnEnableEventTriggerExtension
    {
        public static IUnRegister OnEnableEvent<T>(this T self, Action onEnable, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnEnableEventTrigger>().OnEnableEvent
                       .Register(onEnable, priority);
        }

        public static IUnRegister OnEnableEvent(this GameObject self, Action onEnable, float priority = 0)
        {
            return self.GetOrAddComponent<OnEnableEventTrigger>().OnEnableEvent
                       .Register(onEnable, priority);
        }
    }
}