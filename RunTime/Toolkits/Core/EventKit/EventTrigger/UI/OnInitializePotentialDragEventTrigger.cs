// ------------------------------------------------------------
// @file       OnInitializePotentialDragEventTrigger.cs
// @brief
// @author     zheliku
// @Modified   2025-08-25 02:50:33
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------


namespace Framework3.Toolkits.EventKit
{
    using System;
    using FluentAPI;
    using Core;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class OnInitializePotentialDragEventTrigger : MonoBehaviour, IInitializePotentialDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnInitializePotentialDragEvent = new EasyEvent<PointerEventData>();


        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            OnInitializePotentialDragEvent.Trigger(eventData);
        }
    }

    public static class OnInitializePotentialDragEventTriggerExtension
    {
        public static IUnregister OnInitializePotentialDragEvent<T>(this T self, Action<PointerEventData> onInitializePotentialDrag, float priority = 0)
            where T : Component
        {
            return self.GetOrAddComponent<OnInitializePotentialDragEventTrigger>().OnInitializePotentialDragEvent.Register(onInitializePotentialDrag, priority);
        }

        public static IUnregister OnInitializePotentialDragEvent(this GameObject self, Action<PointerEventData> onInitializePotentialDrag, float priority = 0)
        {
            return self.GetOrAddComponent<OnInitializePotentialDragEventTrigger>().OnInitializePotentialDragEvent.Register(onInitializePotentialDrag, priority);
        }
    }
}