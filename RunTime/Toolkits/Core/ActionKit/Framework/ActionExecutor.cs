// ------------------------------------------------------------
// @file       ActionExecutor.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 22:10:53
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Pool;

    /// <summary>
    /// Action 执行器
    /// </summary>
    internal class ActionExecutor : MonoBehaviour, IActionExecutor
    {
        [ShowInInspector]
        private readonly List<ActionTask> _prepareExecutionTasks = new();

        [ShowInInspector]
        private readonly List<IActionController> _tobeRemovedActions = new();

        [ShowInInspector]
        private readonly Dictionary<IAction, ActionTask> _executingTasks = new();

        private readonly ObjectPool<ActionTask> _actionTaskPool = new(
            createFunc: () => new ActionTask(),
            actionOnGet: null,
            actionOnRelease: task => task.Recycle(),
            actionOnDestroy: null,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100);

        private void Update()
        {
            // 将 _prepareExecutionActions 添加到 _executingActions 中
            foreach (var task in _prepareExecutionTasks)
            {
                _executingTasks[task.Action] = task;
            }

            _prepareExecutionTasks.Clear();

            // 执行 _executingActions 中的每个 action
            foreach (var pair in _executingTasks)
            {
                var task = pair.Value;
                var controller = task.Controller;

                // 选择使用 Time.deltaTime / Time.unscaledDeltaTime
                var deltaTime = controller.UpdateMode == ActionUpdateMode.ScaledDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;

                // 调用 UpdateAction 方法，执行 Action
                if (this.UpdateAction(controller, deltaTime, task.OnFinish))
                {
                    // 执行成功，将 action 添加到 _actionsToRemove 中
                    _tobeRemovedActions.Add(controller);

                    _actionTaskPool.Release(task); // 回收 task
                }
            }

            // 移除执行成功的 action 对应的 controller
            foreach (var controller in _tobeRemovedActions)
            {
                _executingTasks.Remove(controller.Action);
                controller.Recycle(); // 回收 controller 到对应的 Pool 中
            }

            _tobeRemovedActions.Clear();
        }

        public void Execute(IActionController controller, System.Action<IActionController> onFinish = null)
        {
            // 如果 controller 的 Action 状态为已完成，则重置 Action
            if (controller.Action.Status == ActionStatus.Finished)
            {
                controller.Action.Reset();
            }

            // 如果 UpdateAction 方法返回 true，则直接返回
            if (this.UpdateAction(controller, 0, onFinish))
            {
                return;
            }

            // 从 _actionTaskPool 中获取一个任务
            var task = _actionTaskPool.Get();

            task.Action     = controller.Action;
            task.Controller = controller;
            task.OnFinish   = onFinish;

            // 将任务添加到 _prepareExecutionActions 中
            _prepareExecutionTasks.Add(task);
        }

        public void Reset()
        {
            Clear();
        }

        public void Clear()
        {
            _prepareExecutionTasks.Clear();
            _executingTasks.Clear();
            _tobeRemovedActions.Clear();
        }
    }
}