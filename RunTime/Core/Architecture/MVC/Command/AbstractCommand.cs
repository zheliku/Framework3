// ------------------------------------------------------------
// @file       Command.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:21
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using Sirenix.OdinInspector;

    /// <summary>
    /// Command 基类，执行后无返回值
    /// </summary>
    [HideReferenceObjectPicker]
    public abstract class AbstractCommand : ICommand
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

        void ICommand.Execute()
        {
            OnExecute();
        }

        /// <summary>
        /// 执行方法，需要由子类实现
        /// </summary>
        protected abstract void OnExecute();
    }

    /// <summary>
    /// Command 基类，执行后有返回值
    /// </summary>
    [HideReferenceObjectPicker]
    public abstract class AbstractCommand<TResult> : ICommand<TResult>
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

        TResult ICommand<TResult>.Execute()
        {
            return OnExecute();
        }

        /// <summary>
        /// 执行方法，需要由子类实现
        /// </summary>
        protected abstract TResult OnExecute();
    }
}