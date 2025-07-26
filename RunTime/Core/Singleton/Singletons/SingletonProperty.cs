// ------------------------------------------------------------
// @file       SingletonProperty.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:58
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;

    /// <summary> <![CDATA[
    /// 区别于普通的单例实现，TSingleton 可继承其他类，而不是只能继承 Singleton<TSingleton>
    /// ]]> </summary>
    /// <typeparam name="TSingleton"></typeparam>
    public static class SingletonProperty<TSingleton> where TSingleton : class, ISingleton
    {
        /// <summary>
        /// 静态 Lazy
        /// </summary>
        private static readonly Lazy<TSingleton> s_lazyHolder = new(SingletonCreator.CreateSingleton<TSingleton>);

        /// <summary>
        /// 静态属性
        /// </summary>
        public static TSingleton Instance
        {
            get => s_lazyHolder.Value;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public static void Dispose()
        { }
    }
}