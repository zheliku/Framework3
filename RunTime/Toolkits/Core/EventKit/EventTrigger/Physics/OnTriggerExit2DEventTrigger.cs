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

    public class OnTriggerExit2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerExit2DEvent = new EasyEvent<Collider2D>();

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnTriggerExit2DEvent.Trigger(collider);
        }
    }

    public static class OnTriggerExit2DEventTriggerExtension
    {
        public static IUnRegister OnTriggerExit2DEvent<T>(this T self, Action<Collider2D> onTriggerExit2D, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerExit2DEventTrigger>().OnTriggerExit2DEvent
                       .Register(onTriggerExit2D, priority);
        }

        public static IUnRegister OnTriggerExit2DEvent(this GameObject self, Action<Collider2D> onTriggerExit2D, int priority = 0)
        {
            return self.GetOrAddComponent<OnTriggerExit2DEventTrigger>().OnTriggerExit2DEvent
                       .Register(onTriggerExit2D, priority);
        }
    }
}