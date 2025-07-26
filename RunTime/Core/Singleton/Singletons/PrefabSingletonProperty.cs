// ------------------------------------------------------------
// @file       PrefabSingletonProperty.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:46
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class PrefabSingletonProperty<TSingleton> where TSingleton : MonoBehaviour
    {
        public static Func<string, GameObject> PrefabLoader = Resources.Load<GameObject>;

        private static TSingleton s_instance;

        public static TSingleton InstanceWithLoader(Func<string, GameObject> loader)
        {
            PrefabLoader = loader;
            return Instance;
        }

        public static TSingleton Instance
        {
            get
            {
                if (!s_instance)
                {
                    s_instance = Object.FindFirstObjectByType<TSingleton>();
                    if (!s_instance)
                    {
                        var prefab = PrefabLoader?.Invoke(typeof(TSingleton).Name);
                        if (prefab)
                        {
                            s_instance = Object.Instantiate(prefab).GetComponent<TSingleton>();
                            Object.DontDestroyOnLoad(s_instance);
                        }
                    }
                }

                return s_instance;
            }
        }

        public static void Dispose()
        {
            Object.Destroy(s_instance.gameObject);

            s_instance = null;
        }
    }
}