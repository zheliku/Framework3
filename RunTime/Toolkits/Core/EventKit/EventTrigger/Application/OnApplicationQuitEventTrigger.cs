/****************************************************************************
 * Copyright (c) 2015 - 2023 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

namespace Framework3.Toolkits.EventKit
{
    using System;
    using Core;
    using FluentAPI;
    using UnityEngine;

    public class OnApplicationQuitEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnApplicationQuitEvent = new();

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent.Trigger();
        }
    }

    public static class OnApplicationQuitEventTriggerExtension
    {
        public static IUnregister OnApplicationQuitEventEvent<T>(this T self, Action onApplicationQuitEvent, int priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnApplicationQuitEventTrigger>().OnApplicationQuitEvent
               .Register(onApplicationQuitEvent, priority);
        }

        public static IUnregister OnApplicationQuitEventEvent(this GameObject self, Action onApplicationQuitEvent, int priority = 0)
        {
            return self.GetOrAddComponent<OnApplicationQuitEventTrigger>().OnApplicationQuitEvent
               .Register(onApplicationQuitEvent, priority);
        }
    }
}