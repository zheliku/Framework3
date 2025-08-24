// ------------------------------------------------------------
// @file       OnMoveEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:38
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnMoveEventTrigger : MonoBehaviour, IMoveHandler
    {
        public readonly EasyEvent<AxisEventData> OnMoveEvent = new EasyEvent<AxisEventData>();

        public void OnMove(AxisEventData eventData)
        {
            OnMoveEvent.Trigger(eventData);
        }
    }

    public static class OnMoveEventTriggerExtension
    {
        public static IUnRegister OnMoveEvent<T>(this T self, Action<AxisEventData> onMove, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnMoveEventTrigger>().OnMoveEvent.Register(onMove, priority);
        }

        public static IUnRegister OnMoveEvent(this GameObject self, Action<AxisEventData> onMove, float priority = 0)
        {
            return self.GetOrAddComponent<OnMoveEventTrigger>().OnMoveEvent.Register(onMove, priority);
        }
    }
}