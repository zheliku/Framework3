﻿// ------------------------------------------------------------
// @file       UnRegisterCurrentSceneUnloadedTrigger.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 16:10:50
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// 场景卸载时的注销触发器
    /// </summary>
    public sealed class UnRegisterCurrentSceneUnloadedTrigger : UnRegisterTrigger
    {
        private static UnRegisterCurrentSceneUnloadedTrigger s_default;

        public static UnRegisterCurrentSceneUnloadedTrigger Default // 单例模式
        {
            get
            {
                if (!s_default)
                {
                    s_default = new GameObject("UnRegisterCurrentSceneUnloadedTrigger").AddComponent<UnRegisterCurrentSceneUnloadedTrigger>();
                }

                return s_default;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SceneManager.sceneUnloaded += OnSceneUnloaded; // 注册场景卸载事件
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded; // 注销场景卸载事件
        }

        void OnSceneUnloaded(Scene scene)
        {
            UnRegister(); // 场景卸载时注销所有事件
        }
    }
}