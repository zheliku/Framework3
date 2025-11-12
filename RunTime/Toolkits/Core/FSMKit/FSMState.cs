// ------------------------------------------------------------
// @file       CustomState.cs
// @brief      有限状态机（FSM）的灵活状态实现类，支持通过委托注入状态生命周期逻辑
// @author     zheliku
// @Modified   2024-10-20 20:10:46
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FSMKit
{
    using System;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     有限状态机（FSM）的基础状态实现类，实现<see cref="IState" />接口
    ///     支持通过委托注入状态生命周期的各个阶段逻辑（条件检查、进入、更新、退出等）
    ///     提供链式调用API，便于快速构建状态实例
    /// </summary>
    public class FSMState : IState
    {
        /// <summary>
        ///     状态切换条件的委托（返回true表示满足切换条件）
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif // Odin Inspector特性：在编辑器中可视化该委托，便于调试
        private Func<bool> _onCondition;

        /// <summary>
        ///     状态进入时执行的委托
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private Action _onEnter;

        /// <summary>
        ///     状态退出时执行的委托
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private Action _onExit;

        /// <summary>
        ///     状态固定时间更新（物理逻辑）时执行的委托
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private Action _onFixedUpdate;

        /// <summary>
        ///     状态GUI绘制（调试界面）时执行的委托
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private Action _onGUI;

        /// <summary>
        ///     状态每帧更新时执行的委托
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private Action _onUpdate;

        /// <summary>
        ///     检查状态是否满足切换/保持条件（实现<see cref="IState" />接口）
        /// </summary>
        /// <returns>
        ///     若已通过<see cref="OnCondition" />注入委托，则返回委托执行结果；
        ///     若未注入委托，默认返回true（无条件满足）
        /// </returns>
        public bool Condition()
        {
            return _onCondition == null || _onCondition.Invoke();
        }

        /// <summary>
        ///     执行状态进入逻辑（实现<see cref="IState" />接口）
        /// </summary>
        /// <remarks>仅在状态被首次切换进入时调用一次，若未注入<see cref="_onEnter" />委托则不执行任何操作</remarks>
        public void Enter()
        {
            _onEnter?.Invoke();
        }

        /// <summary>
        ///     执行状态每帧更新逻辑（实现<see cref="IState" />接口）
        /// </summary>
        /// <remarks>每帧调用一次（与FSM的Update同步），若未注入<see cref="_onUpdate" />委托则不执行任何操作</remarks>
        public void Update()
        {
            _onUpdate?.Invoke();
        }

        /// <summary>
        ///     执行状态固定时间更新逻辑（实现<see cref="IState" />接口）
        /// </summary>
        /// <remarks>固定时间间隔调用一次（与FSM的FixedUpdate同步），若未注入<see cref="_onFixedUpdate" />委托则不执行任何操作</remarks>
        public void FixedUpdate()
        {
            _onFixedUpdate?.Invoke();
        }

        /// <summary>
        ///     执行状态GUI绘制逻辑（实现<see cref="IState" />接口）
        /// </summary>
        /// <remarks>每帧调用一次（与FSM的OnGUI同步），若未注入<see cref="_onGUI" />委托则不执行任何操作</remarks>
        public void OnGUI()
        {
            _onGUI?.Invoke();
        }

        /// <summary>
        ///     执行状态退出逻辑（实现<see cref="IState" />接口）
        /// </summary>
        /// <remarks>仅在状态被切换退出时调用一次，若未注入<see cref="_onExit" />委托则不执行任何操作</remarks>
        public void Exit()
        {
            _onExit?.Invoke();
        }

        /// <summary>
        ///     注入状态切换的条件检查逻辑（链式调用）
        /// </summary>
        /// <param name="onCondition">条件检查委托：返回true表示满足状态切换/保持条件</param>
        /// <returns>当前<see cref="FSMState" />实例，用于链式调用后续API</returns>
        /// <remarks>若未注入该委托，<see cref="Condition" />方法默认返回true（即无条件满足）</remarks>
        public FSMState OnCondition(Func<bool> onCondition)
        {
            _onCondition = onCondition;
            return this;
        }

        /// <summary>
        ///     注入状态进入时的执行逻辑（链式调用）
        /// </summary>
        /// <param name="onEnter">状态进入委托：在状态被切换进入时执行</param>
        /// <returns>当前<see cref="FSMState" />实例，用于链式调用后续API</returns>
        /// <example>
        ///     <code>
        /// var idleState = new FSMState()
        ///     .OnEnter(() => Debug.Log("进入 idle 状态"))
        ///     .OnUpdate(() => Debug.Log("idle 状态更新"));
        /// </code>
        /// </example>
        public FSMState OnEnter(Action onEnter)
        {
            _onEnter = onEnter;
            return this;
        }

        /// <summary>
        ///     注入状态每帧更新时的执行逻辑（链式调用）
        /// </summary>
        /// <param name="onUpdate">状态更新委托：在FSM的Update方法被调用时执行</param>
        /// <returns>当前<see cref="FSMState" />实例，用于链式调用后续API</returns>
        public FSMState OnUpdate(Action onUpdate)
        {
            _onUpdate = onUpdate;
            return this;
        }

        /// <summary>
        ///     注入状态固定时间更新（物理逻辑）的执行逻辑（链式调用）
        /// </summary>
        /// <param name="onFixedUpdate">状态固定更新委托：在FSM的FixedUpdate方法被调用时执行</param>
        /// <returns>当前<see cref="FSMState" />实例，用于链式调用后续API</returns>
        /// <remarks>建议用于物理相关逻辑（如碰撞检测、力的施加），与Unity的FixedUpdate同步</remarks>
        public FSMState OnFixedUpdate(Action onFixedUpdate)
        {
            _onFixedUpdate = onFixedUpdate;
            return this;
        }

        /// <summary>
        ///     注入状态GUI绘制（调试界面）的执行逻辑（链式调用）
        /// </summary>
        /// <param name="onGUI">GUI绘制委托：在FSM的OnGUI方法被调用时执行</param>
        /// <returns>当前<see cref="FSMState" />实例，用于链式调用后续API</returns>
        /// <remarks>常用于绘制状态调试信息（如当前状态名称、时长），仅在Unity编辑器中生效</remarks>
        public FSMState OnGUI(Action onGUI)
        {
            _onGUI = onGUI;
            return this;
        }

        /// <summary>
        ///     注入状态退出时的执行逻辑（链式调用）
        /// </summary>
        /// <param name="onExit">状态退出委托：在状态被切换退出时执行</param>
        /// <returns>当前<see cref="FSMState" />实例，用于链式调用后续API</returns>
        /// <example>
        ///     <code>
        /// var runState = new FSMState()
        ///     .OnEnter(() => Debug.Log("进入 run 状态"))
        ///     .OnExit(() => Debug.Log("退出 run 状态，重置速度"));
        /// </code>
        /// </example>
        public FSMState OnExit(Action onExit)
        {
            _onExit = onExit;
            return this;
        }
    }
}