// ------------------------------------------------------------
// @file       Action.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 19:10:04
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using UnityEngine.Pool;

    /// <summary>
    /// Action 基类
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    public abstract class AbstractAction<TAction> : IAction where TAction : AbstractAction<TAction>, new()
    {
        private static readonly ObjectPool<TAction> s_pool = new(
            createFunc: () => new TAction(),
            actionOnGet: action =>
            {
                action.ActionID = ActionKit.IDGenerator++;
                action.Deinited = false;
                action.OnCreate();
                action.Reset();
            },
            actionOnRelease: null,
            actionOnDestroy: null,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100);

        protected static TAction CreateInternal()
        {
            return s_pool.Get();
        }

        public ulong ActionID { get; protected set; }

        public ActionStatus Status { get; set; }

        public abstract void OnCreate();

        public abstract void OnStart();

        public abstract void OnExecute(float deltaTime);

        public abstract void OnFinish();

        /// <summary>
        /// 在这里写 Reset 逻辑，外部无法调用 OnReset()，而是调用 Reset()
        /// </summary>
        protected abstract void OnReset();

        /// <summary>
        /// 在这里写 Deinit 逻辑，外部无法调用 OnDeinit()，而是调用 Deinit()
        /// </summary>
        protected abstract void OnDeinit();

        public bool Deinited { get; set; }

        public bool Paused { get; set; }

        public void Reset() // 依赖于 OnReset() 方法
        {
            // 将状态设置为未开始
            Status = ActionStatus.NotStart;

            // 将暂停状态设置为 false
            Paused = false;

            // 调用 OnReset() 方法
            OnReset();
        }

        public void Deinit() // 依赖于 OnDeinit() 方法
        {
            // 如果没有初始化过
            if (!Deinited)
            {
                // 设置反初始化标志
                Deinited = true;

                // 调用 OnDeinit() 方法
                OnDeinit();
                
                s_pool.Release(this as TAction);

                // 将当前对象添加到 ActionQueue 中，以便在适当的时候进行回收
                // ActionQueue.AddCallback(new ActionQueueRecycleCallback<TAction>(s_pool, this as TAction));
            }
        }
    }
}