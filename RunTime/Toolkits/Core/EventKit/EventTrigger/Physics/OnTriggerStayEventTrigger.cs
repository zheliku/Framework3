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

    public class OnTriggerStayEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerStayEvent = new EasyEvent<Collider>();

        private void OnTriggerStay(Collider collider)
        {
            OnTriggerStayEvent.Trigger(collider);
        }
    }

    public static class OnTriggerStayEventTriggerExtension
    {
        public static IUnRegister OnTriggerStayEvent<T>(this T self, Action<Collider> onTriggerStay, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerStayEventTrigger>().OnTriggerStayEvent
                       .Register(onTriggerStay, priority);
        }

        public static IUnRegister OnTriggerStayEvent(this GameObject self, Action<Collider> onTriggerStay, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerStayEventTrigger>().OnTriggerStayEvent
                       .Register(onTriggerStay, priority);
        }
    }
}