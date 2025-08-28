using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using GIB.Auspex;
using GIB;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GIB.EventsProDemo
{
    public class EventProTest : MonoBehaviour
    {
        [InfoBox("This is a UnityEvent as it appears by default.")]
        [Header("UnityEvent Built-In version")]
        public UnityEvent unityEvent;
        [InfoBox("This is an EventPro, which extends the UnityEvent system to be more extensible.")]
        [Header("EventPro version")]
        public EventPro eventPro;

        [Button]
        public void InvokeEvent()
        {
            unityEvent.Invoke();
        }
        [Button(buttonColor: "#ffffc8")]
        public void InvokeEventPro()
        {
            eventPro.Invoke();
        }
    }
}
