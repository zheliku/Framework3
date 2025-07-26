// ------------------------------------------------------------
// @file       ScriptableSingletonProperty.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:35
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
    using UnityEngine;

    public static class ScriptableSingletonProperty<T> where T : ScriptableObject
    {
        public static Func<string, T> ScriptableLoader = Resources.Load<T>;

        private static T s_instance;

        public static T InstanceWithLoader(Func<string, T> loader)
        {
            ScriptableLoader = loader;
            return Instance;
        }

        public static T Instance
        {
            get
            {
                if (s_instance == null) s_instance = ScriptableLoader?.Invoke(typeof(T).Name);
                return s_instance;
            }
        }

        public static void Dispose()
        {
            Resources.UnloadAsset(s_instance);
            
            s_instance = null;
        }
    }
}