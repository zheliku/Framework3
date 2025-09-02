// ------------------------------------------------------------
// @file       UnLoadCurrentSceneUnLoadedTrigger.cs
// @brief
// @author     zheliku
// @Modified   2024-12-10 13:12:50
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class UnloadCurrentSceneUnLoadedTrigger : UnloadTrigger
    {
        private static UnloadCurrentSceneUnLoadedTrigger s_default;

        public static UnloadCurrentSceneUnLoadedTrigger Default
        { // 单例模式
            get
            {
                if (!s_default)
                {
                    s_default = new GameObject("UnregisterCurrentSceneUnloadedTrigger").AddComponent<UnloadCurrentSceneUnLoadedTrigger>();
                }

                return s_default;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            // hideFlags =  HideFlags.HideInHierarchy;
            SceneManager.sceneUnloaded += OnSceneUnloaded; // 注册场景卸载事件
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded; // 注销场景卸载事件
        }

        void OnSceneUnloaded(Scene scene)
        {
            Unload(); // 场景卸载时卸载所有资源
        }
    }
}