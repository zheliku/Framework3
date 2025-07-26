// ------------------------------------------------------------
// @file       ActionKitCurrentScene.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 22:10:44
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using UnityEngine;

    /// <summary>
    /// 用于当前场景的 Action 单例
    /// </summary>
    public class ActionKitCurrentScene : MonoBehaviour
    {
        private static ActionKitCurrentScene s_sceneComponent = null;

        public static ActionKitCurrentScene SceneComponent
        {
            get
            {
                if (!s_sceneComponent)
                {
                    s_sceneComponent = new GameObject(nameof(ActionKitCurrentScene)).AddComponent<ActionKitCurrentScene>();
                }

                return s_sceneComponent;
            }
        }

        private void OnDestroy()
        {
            s_sceneComponent = null;
        }
    }
}