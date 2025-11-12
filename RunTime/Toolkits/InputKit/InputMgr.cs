// ------------------------------------------------------------
// @file       InputMgr.cs
// @brief
// @author     zheliku
// @Modified   2024-12-26 22:12:33
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System.Collections.Generic;
    using SingletonKit;
    using UnityEngine.InputSystem;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [MonoSingletonPath("Framework3/InputKit")]
    public class InputMgr : MonoSingleton<InputMgr>
    {
    #region 字段

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public Dictionary<string, InputActionMapTracker> ActionMapTrackerDict { get; } = new();

    #endregion

    #region 公共方法

        public override void OnSingletonInit()
        {
            // 初始化时，将所有 inputAction 记录到 ActionMaps 中
            var inputActionAsset = InputSystem.actions;
            foreach (var map in inputActionAsset.actionMaps)
            {
                var tracker = new InputActionMapTracker();
                foreach (var action in map)
                {
                    tracker.InputActions[action.name] = new InputActionTracker();
                }
                ActionMapTrackerDict.TryAdd(map.name, tracker);
            }
        }

        /// <summary>
        ///     获取 InputAction 对应的 Mono
        /// </summary>
        /// <param name="action">输入行为</param>
        /// <returns>InputAction 对应的 Mono</returns>
        public static InputActionTracker GetInputActionTracker(InputAction action)
        {
            return Instance.ActionMapTrackerDict[action.actionMap.name].InputActions[action.name];
        }

        /// <summary>
        ///     获取 InputAction 对应的 MapMono
        /// </summary>
        /// <param name="action">输入行为</param>
        /// <returns>InputAction 对应的 MapMono</returns>
        public static InputActionMapTracker GetInputActionMapTracker(InputAction action)
        {
            return Instance.ActionMapTrackerDict[action.actionMap.name];
        }

        /// <summary>
        ///     获取 InputActionMap 对应的 MapMono
        /// </summary>
        /// <param name="actionMap">输入行为地图</param>
        /// <returns>InputAction 对应的 MapMono</returns>
        public static InputActionMapTracker GetInputActionMapTracker(InputActionMap actionMap)
        {
            return Instance.ActionMapTrackerDict[actionMap.name];
        }

    #endregion
    }
}