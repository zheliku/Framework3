// ------------------------------------------------------------
// @file       IActionExecutorExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 22:10:10
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using System;
    using UnityEngine;

    public static class ActionExecutorExtension
    {
        public static bool UpdateAction(
            this IActionExecutor             self,
            IActionController                controller,
            float                            deltaTime,
            Action<IActionController> onFinish = null)
        {
            // 如果控制器中的动作未执行完成，并且执行动作成功
            if (!controller.Action.Deinited && controller.Action.Execute(deltaTime))
            {
                onFinish?.Invoke(controller); // 执行回调
                controller.Deinit();          // 执行完成后的反初始化
                return true;
            }

            return controller.Action.Deinited;
        }

        public static IAction ExecuteByUpdate(
            this GameObject                  self,
            IAction                          action,
            IActionController                controller,
            Action<IActionController> onFinish = null)
        {
            if (action.Status == ActionStatus.Finished)
            {
                action.Reset();
            }
            
            var comp = self.gameObject.GetComponent<ActionExecutor>();
            if (!comp)
            {
                comp = self.gameObject.AddComponent<ActionExecutor>();
            }
            
            comp.Execute(controller, onFinish); // 挂载 ActionExecutor 帧更新执行 Action
            return action;
        }
    }
}