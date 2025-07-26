// ------------------------------------------------------------
// @file       Architecture.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:35
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

using Sirenix.OdinInspector;

namespace Framework3.Core
{
    using System;
    using System.Linq;

    public abstract class AbstractArchitecture<TArchitecture> : IArchitecture where TArchitecture : AbstractArchitecture<TArchitecture>, new()
    {
    #region Static

        /// <summary>
        /// 注册补丁的回调
        /// </summary>
        public static Action<TArchitecture> OnRegisterPatch = architecture => { };

        protected static TArchitecture s_architecture;

        /// <summary>
        /// 确保架构实例存在，若不存在则创建新的实例并初始化
        /// </summary>
        static void MakeSureArchitecture()
        {
            if (s_architecture == null)
            {
                // 如果架构为空，则创建新的架构
                s_architecture = new TArchitecture();

                s_architecture.Init(); // 初始化架构

                OnRegisterPatch?.Invoke(s_architecture); // 如果有注册补丁的回调函数，则调用

                // 遍历架构中的所有模型，如果模型未初始化，则初始化
                foreach (var model in s_architecture._iocContainer.GetInstancesByType<IModel>().Where<IModel>(m => !m.Initialized))
                {
                    model.Init();
                }

                // 遍历架构中的所有系统，如果系统未初始化，则初始化
                foreach (var system in s_architecture._iocContainer.GetInstancesByType<ISystem>().Where<ISystem>(s => !s.Initialized))
                {
                    system.Init();
                }

                // 设置架构已经初始化
                s_architecture._initialized = true;
            }
        }

        /// <summary>
        /// 获取当前架构实例
        /// </summary>
        public static IArchitecture Architecture
        {
            get
            {
                MakeSureArchitecture(); // 如果架构为空，则创建新的架构
                return s_architecture;
            }
        }

    #endregion

    #region 字段
        
        [ShowInInspector]
        private bool _initialized = false;

        [ShowInInspector]
        private IOCContainer _iocContainer = new IOCContainer();

        // 一个 Architecture 对应一个 TypeEventSystem
        [ShowInInspector]
        private TypeEventSystem _typeEventSystem = new TypeEventSystem();

    #endregion

    #region 公共方法

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.Architecture = this;
            _iocContainer.Register<TSystem>(system);

            if (_initialized)
            {
                // 若 Architecture 已初始化，则新注册的 System 也需要立即初始化
                system.Init();
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.Architecture = this;
            _iocContainer.Register<TModel>(model);

            if (_initialized)
            {
                // 若 Architecture 已初始化，则新注册的 Model 也需要立即初始化
                model.Init();
            }
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            _iocContainer.Register<TUtility>(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return _iocContainer.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return _iocContainer.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return _iocContainer.Get<TUtility>();
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        public TResult SendCommand<TResult>(ICommand<TResult> command)
        {
            return ExecuteCommand(command);
        }
        
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            return DoQuery<TResult>(query);
        }

        public void SendEvent<TEvent>() where TEvent : new()
        {
            _typeEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            _typeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent, float priority)
        {
            return _typeEventSystem.Register<TEvent>(onEvent, priority);
        }
        
        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            _typeEventSystem.UnRegister<TEvent>(onEvent);
        }

        public void Deinit()
        {
            OnDeinit(); // 调用反初始化事件

            // 遍历所有已初始化的系统，调用其反初始化方法
            foreach (var system in _iocContainer.GetInstancesByType<ISystem>().Where<ISystem>(s => s.Initialized)) { system.Deinit(); }

            // 遍历所有已初始化的模型，调用其反初始化方法
            foreach (var model in _iocContainer.GetInstancesByType<IModel>().Where<IModel>(m => m.Initialized)) { model.Deinit(); }

            _iocContainer.Clear(); // 清空 IOC 容器
            _initialized = false; // 设置初始化状态为 false
        }

    #endregion

    #region 私有方法

        protected abstract void Init();

        protected virtual void OnDeinit() { }

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.Architecture = this;
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.Architecture = this;
            command.Execute();
        }

        protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
        {
            query.Architecture = this;
            return query.Do();
        }

    #endregion
    }
}