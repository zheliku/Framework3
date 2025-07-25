﻿// ------------------------------------------------------------
// @file       TypeEventSystemBasicExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-13 15:10:30
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core.Example._2.TypeEventSystem._2._0.Basic
{
    using UnityEngine;
    using TypeEventSystem = Core.TypeEventSystem;

    public class TypeEventSystemBasicExample : MonoBehaviour
    {
        // 最好使用 struct 声明，减少 GC
        public struct TestEventA
        {
            public int Age;

            public override string ToString() { return $"{nameof(TestEventA)}: Age={Age}"; }
        }

        private void Start()
        {
            TypeEventSystem.Global.Register<TestEventA>(e =>
            {
                Debug.Log(e);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Send(new TestEventA())", GUILayout.Width(200), GUILayout.Height(50)))
            {
                TypeEventSystem.Global.Send(new TestEventA()
                {
                    Age = 18
                });
            }

            if (GUILayout.Button("Send<TestEventA>()", GUILayout.Width(200), GUILayout.Height(50)))
            {
                TypeEventSystem.Global.Send<TestEventA>();
            }
        }
    }
}