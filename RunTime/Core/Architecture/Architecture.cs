// ------------------------------------------------------------
// @file       Architecture.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:35
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System;
    using System.Linq;
    using Sirenix.OdinInspector;

    [HideReferenceObjectPicker]
    public abstract class AbstractArchitecture<TArchitecture> : IArchitecture where TArchitecture : AbstractArchitecture<TArchitecture>, new()
    {
    #region Static

        /// <summary>
        /// 注册补丁的回调
        /// </summary>
        public static Action<TArchitecture> OnRegisterPatch = architecture => { };

        protected static TArchitecture s_architecture;

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
                s_architecture._inited = true;
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
        private bool _inited = false;

        [ShowInInspector]
        private IOCContainer _iocContainer = new IOCContainer();

        // 一个 Architecture 对应一个 TypeEventSystem
        [ShowInInspector]
        private TypeEventSystem _typeEventSystem = new TypeEventSystem();

    #endregion

    #region 公共方法

        /// <summary>
        /// 注册 System
        /// </summary>
        /// <param name="system">System 实例</param>
        /// <typeparam name="TSystem">System 类型</typeparam>
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            _iocContainer.Register<TSystem>(system);

            if (_inited)
            {
                // 若 Architecture 已初始化，则新注册的 System 也需要立即初始化
                system.Init();
            }
        }

        /// <summary>
        /// 注册 Model
        /// </summary>
        /// <param name="model">Model 实例</param>
        /// <typeparam name="TModel">Model 类型</typeparam>
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            _iocContainer.Register<TModel>(model);

            if (_inited)
            {
                // 若 Architecture 已初始化，则新注册的 Model 也需要立即初始化
                model.Init();
            }
        }

        /// <summary>
        /// 注册 TUtility
        /// </summary>
        /// <param name="utility">TUtility 实例</param>
        /// <typeparam name="TUtility">TUtility 类型</typeparam>
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            _iocContainer.Register<TUtility>(utility);
        }

        /// <summary>
        /// 获取已注册的 System
        /// </summary>
        /// <typeparam name="TSystem">System 类型</typeparam>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return _iocContainer.Get<TSystem>();
        }

        /// <summary>
        /// 获取已注册的 System
        /// </summary>
        /// <typeparam name="TModel">Model 类型</typeparam>
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return _iocContainer.Get<TModel>();
        }

        /// <summary>
        /// 获取已注册的 System
        /// </summary>
        /// <typeparam name="TUtility">Utility 类型</typeparam>
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return _iocContainer.Get<TUtility>();
        }

        /// <summary>
        /// 发送 Command
        /// </summary>
        /// <param name="command">Command 实例</param>
        /// <typeparam name="TCommand">Command 类型</typeparam>
        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        /// <summary>
        /// 发送带有返回值的 Command&lt;TResult&gt;
        /// </summary>
        /// <param name="command">Command&lt;TResult&gt; 实例</param>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <returns>返回值</returns>
        public TResult SendCommand<TResult>(ICommand<TResult> command)
        {
            return ExecuteCommand(command);
        }
        
        /// <summary>
        /// 发送查询请求，返回查询结果。
        /// </summary>
        /// <typeparam name="TResult">查询结果类型</typeparam>
        /// <param name="query">查询对象</param>
        /// <returns>查询结果</returns>
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            return DoQuery<TResult>(query);
        }

        /// <summary>
        /// 发送无参数事件。
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        public void SendEvent<TEvent>() where TEvent : new()
        {
            _typeEventSystem.Send<TEvent>();
        }

        /// <summary>
        /// 发送带参数事件。
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="e">事件参数</param>
        public void SendEvent<TEvent>(TEvent e)
        {
            _typeEventSystem.Send<TEvent>(e);
        }

        /// <summary>
        /// 注册事件监听器。
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="onEvent">事件回调</param>
        /// <param name="priority">优先级</param>
        /// <returns>注销注册的接口</returns>
        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent, int priority)
        {
            return _typeEventSystem.Register<TEvent>(onEvent, priority);
        }

        /// <summary>
        /// 注销事件监听器。
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="onEvent">事件回调</param>
        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            _typeEventSystem.UnRegister<TEvent>(onEvent);
        }

        /// <summary>
        /// 反初始化架构，释放所有资源。
        /// </summary>
        public void Deinit()
        {
            OnDeinit(); // 调用反初始化事件

            // 遍历所有已初始化的系统，调用其反初始化方法
            foreach (var system in _iocContainer.GetInstancesByType<ISystem>().Where<ISystem>(s => s.Initialized)) { system.Deinit(); }

            // 遍历所有已初始化的模型，调用其反初始化方法
            foreach (var model in _iocContainer.GetInstancesByType<IModel>().Where<IModel>(m => m.Initialized)) { model.Deinit(); }

            _iocContainer.Clear(); // 清空 IOC 容器
            _inited = false; // 设置初始化状态为 false
        }

    #endregion

    #region 私有方法

        protected abstract void Init();

        protected virtual void OnDeinit() { }

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetArchitecture(this);
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

    #endregion
    }
}