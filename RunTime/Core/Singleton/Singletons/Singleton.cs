// ------------------------------------------------------------
// @file       Singleton.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 17:10:13
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;

    public abstract class Singleton<TSingleton> : ISingleton where TSingleton : Singleton<TSingleton>
    {
        /// <summary>
        /// 静态 Lazy
        /// </summary>
        protected static readonly Lazy<TSingleton> s_lazyHolder = new(SingletonCreator.CreateSingleton<TSingleton>);

        /// <summary>
        /// 静态属性
        /// </summary>
        public static TSingleton Instance
        {
            get => s_lazyHolder.Value;
        }

        /// <summary>
        /// 单例初始化方法
        /// </summary>
        public virtual void OnSingletonInit()
        { }

        /// <summary>
        /// 资源释放
        /// </summary>
        public virtual void Dispose()
        { }
    }
}