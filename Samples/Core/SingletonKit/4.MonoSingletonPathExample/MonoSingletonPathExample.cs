﻿// ------------------------------------------------------------
// @file       MonoSingletonPathExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:35
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.SingletonKit.Example._4.MonoSingletonPathExample
{
    using UnityEngine;
    using Framework3.Core;

    public class MonoSingletonPathExample : MonoBehaviour
    {
        private void Start()
        {
            var instance = ClassUseMonoSingletonPath.Instance;
        }

        [MonoSingletonPath("[Example]/MonoSingletonPath")]
        internal class ClassUseMonoSingletonPath : MonoSingleton<ClassUseMonoSingletonPath>
        { }
    }
}