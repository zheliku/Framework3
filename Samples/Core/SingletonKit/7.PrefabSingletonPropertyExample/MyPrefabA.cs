﻿// ------------------------------------------------------------
// @file       MyPrefabA.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:19
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.SingletonKit.Example._7.PrefabSingletonPropertyExample
{
    using UnityEngine;
    using Framework3.Core;

    public class MyPrefabA : MonoBehaviour
    {
        public static MyPrefabA Instance => PrefabSingletonProperty<MyPrefabA>
           .InstanceWithLoader(prefabName => Resources.Load<GameObject>("Prefabs/" + prefabName));

        private void OnDestroy()
        {
            PrefabSingletonProperty<MyPrefabA>.Dispose();
        }
    }
}
