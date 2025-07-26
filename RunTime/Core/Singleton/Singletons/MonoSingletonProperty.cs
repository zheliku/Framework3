// ------------------------------------------------------------
// @file       MonoSingletonProperty.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:25
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using UnityEngine;

    /// <summary>
    /// 通过属性实现的 MonoSingleton，不占用父类的位置
    /// </summary>
    public static class MonoSingletonProperty<TSingleton> where TSingleton : MonoBehaviour, ISingleton
    {
        private static TSingleton s_instance;

        public static TSingleton Instance
        {
            get
            {
                if (null == s_instance)
                {
                    s_instance = SingletonCreator.CreateMonoSingleton<TSingleton>();
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