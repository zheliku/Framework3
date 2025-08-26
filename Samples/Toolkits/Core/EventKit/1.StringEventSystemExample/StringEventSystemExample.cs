// ------------------------------------------------------------
// @file       StringEventSystemExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-23 16:10:43
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.EventKit.Example._1.StringEventSystemExample
{
    using Framework3.Core;
    using UnityEngine;

    public class StringEventSystemExample : MonoBehaviour
    {
        void Start()
        {
            StringEventSystem.Global.Register<string>(nameof(OnEventA), OnEventA).UnregisterWhenGameObjectDestroyed(gameObject);

            // 事件 + 参数
            StringEventSystem.Global.Register<string, int>(nameof(OnEventB), OnEventB).UnregisterWhenGameObjectDestroyed(gameObject);
        }
        
        private void OnGUI()
        {
            if (GUILayout.Button("TestEventA", GUILayout.Width(150), GUILayout.Height(50)))
            {
                StringEventSystem.Global.Send<string>(nameof(OnEventA), "OnEventA");
            }

            if (GUILayout.Button("TestEventB", GUILayout.Width(150), GUILayout.Height(50)))
            {
                StringEventSystem.Global.Send<string, int>(nameof(OnEventB), "OnEventB", 10);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StringEventSystem.Global.Send("TEST_ONE");
                StringEventSystem.Global.Send("TEST_TWO", 10);
            }
        }
        
        void OnEventA(string obj)
        {
            Debug.Log($"OnEventA: {obj}");
        }
        
        void OnEventB(string obj, int i)
        {
            Debug.Log($"OnEventB: {obj}, {i}");
        }
    }
}