// ------------------------------------------------------------
// @file       PrefabSingletonPropertyExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:57
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.SingletonKit.Example._7.PrefabSingletonPropertyExample
{
    using UnityEngine;

    public class PrefabSingletonPropertyExample : MonoBehaviour
    {
        private void Start()
        {
            var prefabA = MyPrefabA.Instance;
        }
    }
}
