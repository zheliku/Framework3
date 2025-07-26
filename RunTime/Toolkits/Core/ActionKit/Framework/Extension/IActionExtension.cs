// ------------------------------------------------------------
// @file       IActionExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 20:10:02
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using System;
    using UnityEngine;

    public static class IActionExtension
    {
        public static IActionController Start(this IAction self, Component component, Action<IActionController> onFinish = null)
        {
            return StartInternal(self, component.gameObject, onFinish);
        }

        public static IActionController Start(this IAction self, Component component, Action onFinish)
        {
            return StartInternal(self, component.gameObject, _ => onFinish());
        }

        public static IActionController Start(this IAction self, GameObject gameObject, Action<IActionController> onFinish = null)
        {
            return StartInternal(self, gameObject, onFinish);
        }

        public static IActionController Start(this IAction self, GameObject gameObject, Action onFinish)
        {
            return StartInternal(self, gameObject, _ => onFinish());
        }

        public static IActionController StartCurrentScene(this IAction self, Action<IActionController> onFinish = null)
        {
            return StartInternal(self, ActionKitCurrentScene.SceneComponent.gameObject, onFinish);
        }

        public static IActionController StartCurrentScene(this IAction self, Action onFinish)
        {
            return StartInternal(self, ActionKitCurrentScene.SceneComponent.gameObject, _ => onFinish());
        }

        public static IActionController StartGlobal(this IAction self, Action<IActionController> onFinish = null)
        {
            return StartInternal(self, ActionKitMonoBehaviourEvent.Instance.gameObject, onFinish);
        }

        public static IActionController StartGlobal(this IAction self, Action onFinish)
        {
            return StartInternal(self, ActionKitMonoBehaviourEvent.Instance.gameObject, _ => onFinish());
        }

        public static void Pause(this IActionController self)
        {
            if (self.ActionID == self.Action.ActionID)
            {
                self.Action.Paused = true;
            }
        }

        public static void Resume(this IActionController self)
        {
            if (self.ActionID == self.Action.ActionID)
            {
                self.Action.Paused = false;
            }
        }

        public static void Finish(this IAction self)
        {
            self.Status = ActionStatus.Finished;
        }

        /// <summary>
        /// 执行 Action 方法
        /// </summary>
        /// <param name="self">Action 实例</param>
        /// <param name="deltaTime">当前帧间隔时间</param>
        /// <returns>是否执行完成</returns>
        public static bool Execute(this IAction self, float deltaTime)
        {
            if (self.Status == ActionStatus.NotStart)
            {
                self.OnStart();

                if (self.Status == ActionStatus.Finished)
                {
                    self.OnFinish();
                    return true; // Finish 后才会 return true
                }

                self.Status = ActionStatus.Started;
            }
            else if (self.Status == ActionStatus.Started)
            {
                if (self.Paused) return false;

                self.OnExecute(deltaTime);

                if (self.Status == ActionStatus.Finished)
                {
                    self.OnFinish();
                    return true; // Finish 后才会 return true
                }
            }
            else if (self.Status == ActionStatus.Finished)
            {
                self.OnFinish();
                return true; // Finish 后才会 return true
            }

            return false;
        }

        private static IActionController StartInternal(IAction action, GameObject gameObject, Action<IActionController> onFinish = null)
        {
            var controller = ActionController.Spawn();
            controller.ActionID   = action.ActionID;
            controller.Action     = action;
            controller.UpdateMode = ActionUpdateMode.ScaledDeltaTime;
            gameObject.ExecuteByUpdate(action, controller, onFinish);
            return controller;
        }
    }
}