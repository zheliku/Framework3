// ------------------------------------------------------------
// @file       ActionQueueRecycleCallback.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 20:10:41
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using UnityEngine.Pool;

    public struct ActionQueueRecycleCallback<T> : IActionQueueCallback where T : class, IAction
    {
        /// <summary>
        /// 需要回收倒哪个 Pool 中
        /// </summary>
        private ObjectPool<T> _pool;

        /// <summary>
        /// 哪个 Action 需要回收
        /// </summary>
        private T _action;

        public ActionQueueRecycleCallback(ObjectPool<T> pool, T action)
        {
            _pool = pool;
            _action = action;
        }
        
        /// <summary>
        /// 回收方法
        /// </summary>
        public void Call()
        {
            _pool.Release(_action);
            _pool   = null;
            _action = null;
        }
    }
}