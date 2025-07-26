// ------------------------------------------------------------
// @file       PersistentMonoSingleton.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:19
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using UnityEngine;

    /// <summary>
    /// 当场景里包含两个 PersistentMonoSingleton，保留先创建的
    /// </summary>
    public abstract class PersistentMonoSingleton<TSingleton> : MonoBehaviour where TSingleton : Component
    {
        protected static TSingleton s_instance;

        public static TSingleton Instance
        {
            get
            {
                if (s_instance)
                {
                    return s_instance;
                }
                
                s_instance = FindFirstObjectByType<TSingleton>();
                if (s_instance)
                {
                    return s_instance;
                }
                
                var obj = new GameObject();
                s_instance = obj.AddComponent<TSingleton>();
                return s_instance;
            }
        }

        // 当场景里包含两个 PersistentMonoSingleton，保留先创建的
        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (s_instance == null)
            {
                s_instance = this as TSingleton;
                DontDestroyOnLoad(transform.gameObject);
            }
            else
            {
                if (this != s_instance)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}