// ------------------------------------------------------------
// @file       Query.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:39
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     Query 基类
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public abstract class Query<TResult> : IQuery<TResult>
    {
        public IArchitecture Architecture
        {
            get;
            private set;
        }

        IArchitecture ICanSetArchitecture.Architecture
        {
            set => Architecture = value;
        }

        public TResult Do() { return OnDo(); }

        /// <summary>
        ///     查询方法，需要由子类实现
        /// </summary>
        /// <returns>查询结果</returns>
        protected abstract TResult OnDo();
    }
}