﻿// ------------------------------------------------------------
// @file       Custom.cs
// @brief
// @author     zheliku
// @Modified   2024-10-29 10:10:53
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using System;
    using UnityEngine.Pool;

    public interface ICustomAPI<TData>
    {
        TData Data { get; set; }

        ICustomAPI<TData> OnStart(Action onStart);

        ICustomAPI<TData> OnExecute(Action<float> onExecute);

        ICustomAPI<TData> OnFinish(Action onFinish);

        void Finish();
    }

    public class Custom<TData> : AbstractAction<Custom<TData>>, ICustomAPI<TData>
    {
    #region Static

        public static Custom<TData> Create()
        {
            return CreateInternal();
        }

    #endregion

    #region 字段

        protected Action _onStart;
        protected Action<float> _onExecute;
        protected Action _onFinish;

    #endregion

    #region 接口

        public TData Data { get; set; }

        public ICustomAPI<TData> OnStart(Action onStart)
        {
            _onStart = onStart;
            return this;
        }

        public ICustomAPI<TData> OnExecute(Action<float> onExecute)
        {
            _onExecute = onExecute;
            return this;
        }

        public ICustomAPI<TData> OnFinish(Action onFinish)
        {
            _onFinish = onFinish;
            return this;
        }

        public void Finish()
        {
            Status = ActionStatus.Finished;
        }

        public override void OnCreate() { }


        public override void OnStart()
        {
            _onStart?.Invoke();
        }

        public override void OnExecute(float deltaTime)
        {
            _onExecute?.Invoke(deltaTime);
        }

        public override void OnFinish()
        {
            _onFinish?.Invoke();
        }

        protected override void OnReset() { }

        protected override void OnDeinit()
        {
            _onStart   = null;
            _onExecute = null;
            _onFinish  = null;
        }

    #endregion
    }

    public class Custom : Custom<object>
    {
        protected Custom() { }

        private static readonly ObjectPool<Custom> s_objectPool = new(
            createFunc: () => new Custom(),
            actionOnGet: custom =>
            {
                custom.Deinited = false;
                custom.Reset();
            },
            actionOnRelease: null,
            actionOnDestroy: null,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100);

        public new static Custom Create()
        {
            return s_objectPool.Get();
        }

        public new void Deinit()
        {
            if (!Deinited)
            {
                Deinited   = true;
                _onStart   = null;
                _onExecute = null;
                _onFinish  = null;

                s_objectPool.Release(this);
            }
        }
    }
}