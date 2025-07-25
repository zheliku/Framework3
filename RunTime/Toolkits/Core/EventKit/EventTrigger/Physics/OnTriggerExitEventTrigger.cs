﻿/****************************************************************************
 * Copyright (c) 2016 - 2022 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using Framework3.Core;

namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Framework3.Core;
    using UnityEngine;

    public class OnTriggerExitEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerExitEvent = new EasyEvent<Collider>();

        private void OnTriggerExit(Collider collider)
        {
            OnTriggerExitEvent.Trigger(collider);
        }
    }

    public static class OnTriggerExitEventTriggerExtension
    {
        public static IUnRegister OnTriggerExitEvent<T>(this T self, Action<Collider> onTriggerExit, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerExitEventTrigger>().OnTriggerExitEvent
                       .Register(onTriggerExit, priority);
        }

        public static IUnRegister OnTriggerExitEvent(this GameObject self, Action<Collider> onTriggerExit, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerExitEventTrigger>().OnTriggerExitEvent
                       .Register(onTriggerExit, priority);
        }
    }
}