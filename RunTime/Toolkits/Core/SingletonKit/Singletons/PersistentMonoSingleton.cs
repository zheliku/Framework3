// ------------------------------------------------------------
// @file       PersistentMonoSingleton.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:19
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.SingletonKit
{
    using UnityEngine;

    /// <summary>
    /// 当场景里包含两个 PersistentMonoSingleton，保留先创建的
    /// </summary>
    public abstract class PersistentMonoSingleton<TMonoSingleton> : MonoSingleton<TMonoSingleton> where TMonoSingleton : PersistentMonoSingleton<TMonoSingleton>
    {
        // 当场景里包含两个 PersistentMonoSingleton，保留先创建的
        public virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!s_instance)
            {
                s_instance = this as TMonoSingleton;
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