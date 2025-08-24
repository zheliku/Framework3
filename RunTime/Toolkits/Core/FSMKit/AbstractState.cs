// ------------------------------------------------------------
// @file       State.cs
// @brief      有限状态机框架的抽象状态基类
// @author     zheliku
// @Modified   2024-10-20 20:10:36
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FSMKit
{
    /// <summary>
    /// 状态机模式中的抽象状态基类，实现了IState接口
    /// 提供状态生命周期的基本框架，用于派生具体状态类
    /// </summary>
    /// <typeparam name="TStateId">状态ID的类型，用于标识不同状态</typeparam>
    /// <typeparam name="TOwner">状态拥有者的类型，即状态所属的对象类型</typeparam>
    public abstract class AbstractState<TStateId, TOwner> : IState
    {
        /// <summary>
        /// 所属的有限状态机实例
        /// </summary>
        protected readonly FSM<TStateId> _fsm;

        /// <summary>
        /// 状态的拥有者实例
        /// </summary>
        protected TOwner _owner;

        /// <summary>
        /// 初始化状态的新实例
        /// </summary>
        /// <param name="fsm">当前状态所属的状态机</param>
        /// <param name="owner">状态的拥有者对象</param>
        public AbstractState(FSM<TStateId> fsm, TOwner owner)
        {
            _fsm = fsm;
            _owner = owner;
        }

        /// <summary>
        /// 实现IState接口的条件检查方法，内部调用OnCondition()
        /// </summary>
        /// <returns>如果条件满足状态转换条件则返回true，否则返回false</returns>
        bool IState.Condition()
        {
            return OnCondition();
        }

        /// <summary>
        /// 实现IState接口的进入状态方法，内部调用OnEnter()
        /// </summary>
        void IState.Enter()
        {
            OnEnter();
        }

        /// <summary>
        /// 实现IState接口的更新方法，内部调用OnUpdate()
        /// </summary>
        void IState.Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// 实现IState接口的固定更新方法，内部调用OnFixedUpdate()
        /// </summary>
        void IState.FixedUpdate()
        {
            OnFixedUpdate();
        }

        /// <summary>
        /// 实现IState接口的GUI绘制方法，内部调用OnGUI()
        /// </summary>
        void IState.OnGUI()
        {
            OnGUI();
        }

        /// <summary>
        /// 实现IState接口的退出状态方法，内部调用OnExit()
        /// </summary>
        void IState.Exit()
        {
            OnExit();
        }

        /// <summary>
        /// 状态条件检查，用于判断是否满足进入或保持该状态的条件
        /// </summary>
        /// <returns>默认返回true，表示满足条件</returns>
        protected virtual bool OnCondition()
        {
            return true;
        }

        /// <summary>
        /// 进入状态时调用的方法，用于初始化状态
        /// </summary>
        protected virtual void OnEnter()
        { }

        /// <summary>
        /// 每帧更新时调用的方法，用于处理状态的逻辑更新
        /// </summary>
        protected virtual void OnUpdate()
        { }

        /// <summary>
        /// 固定时间间隔更新时调用的方法，用于处理物理相关的逻辑
        /// </summary>
        protected virtual void OnFixedUpdate()
        { }

        /// <summary>
        /// GUI绘制时调用的方法，用于调试或绘制状态相关的界面元素
        /// </summary>
        public virtual void OnGUI()
        { }

        /// <summary>
        /// 退出状态时调用的方法，用于清理状态资源
        /// </summary>
        protected virtual void OnExit()
        { }
    }
}