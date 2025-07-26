/****************************************************************************
 * Copyright (c) 2016 - 2023 liangxiegame UNDER MIT License
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
    using UnityEngine.EventSystems;

    public class OnSelectEventTrigger : MonoBehaviour, ISelectHandler
    {
        public readonly EasyEvent<BaseEventData> OnSelectEvent = new EasyEvent<BaseEventData>();

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectEvent.Trigger(eventData);
        }
    }

    public static class OnSelectEventTriggerTriggerExtension
    {
        public static IUnRegister OnSelectEvent<T>(this T self, Action<BaseEventData> onSelect, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnSelectEventTrigger>().OnSelectEvent.Register(onSelect, priority);
        }

        public static IUnRegister OnSelectEvent(this GameObject self, Action<BaseEventData> onSelect, float priority = 0)
        {
            return self.GetOrAddComponent<OnSelectEventTrigger>().OnSelectEvent.Register(onSelect, priority);
        }
    }
}