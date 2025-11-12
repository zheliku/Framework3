// ------------------------------------------------------------
// @file       Model.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:49
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     Model 基类
    /// </summary>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public abstract class AbstractModel : IModel
    {
        private IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.Architecture
        {
            get => _architecture;
        }

        IArchitecture ICanSetArchitecture.Architecture
        {
            set => _architecture = value;
        }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public bool Initialized { get; protected set; }

        void ICanInit.Init()
        {
            OnInit();
            Initialized = true;
        }

        public void Deinit()
        {
            OnDeinit();
            Initialized = false;
        }

        /// <summary>
        ///     初始化方法，需要由子类实现
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        ///     反初始化方法
        /// </summary>
        protected virtual void OnDeinit() { }
    }
}