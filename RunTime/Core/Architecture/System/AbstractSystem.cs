// ------------------------------------------------------------
// @file       System.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:20
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using Sirenix.OdinInspector;

    [HideReferenceObjectPicker]
    public abstract class AbstractSystem : ISystem
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

        [ShowInInspector]
        public bool Initialized { get; protected set; }

        // 仅能通过 ICanInit 接口使用 Init 方法
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
        /// 初始化方法，需要由子类实现
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// 反初始化方法
        /// </summary>
        protected virtual void OnDeinit() { }
    }
}