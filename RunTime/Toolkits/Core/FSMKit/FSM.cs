// ------------------------------------------------------------
// @file       FSM.cs
// @brief      有限状态机（FSM）核心实现类，用于管理状态注册、切换与生命周期
// @author     zheliku
// @Modified   2024-10-20 20:10:55
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FSMKit
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     有限状态机（FSM）核心类，负责状态的注册、存储、切换及生命周期管理
    ///     支持状态切换回调、状态时长统计，依赖Odin Inspector提供编辑器可视化
    /// </summary>
    /// <typeparam name="TStateId">状态ID的类型（如枚举、int等），用于唯一标识每个状态</typeparam>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif // Odin Inspector特性：隐藏引用对象选择器，简化编辑器界面
    public sealed class FSM<TStateId>
    {
    #region 字段

        /// <summary>
        ///     存储所有状态的字典：Key为状态ID，Value为对应的状态实例
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif // Odin Inspector特性：在编辑器中显示该私有字段
        private readonly Dictionary<TStateId, IState> _states = new();

        /// <summary>
        ///     当前激活的状态实例
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private IState _currentState;

        /// <summary>
        ///     当前激活状态的ID
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private TStateId _currentStateId;

        /// <summary>
        ///     上一个激活状态的ID（用于状态切换回溯）
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private TStateId _previousStateId;

        /// <summary>
        ///     状态切换时触发的回调函数：参数1为上一个状态ID，参数2为当前状态ID
        /// </summary>
        private Action<TStateId, TStateId> _onStateChanged = (_, _) => { };

        /// <summary>
        ///     当前状态已持续的帧数（从进入状态开始计数，每帧Update递增）
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private long _frameCountOfCurrentState = 1;

        /// <summary>
        ///     当前状态已持续的时间（单位：秒，从进入状态开始计时，每帧累加Time.deltaTime）
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private float _secondsOfCurrentState;

    #endregion

    #region 属性

        /// <summary>
        ///     获取当前激活的状态实例（只读）
        /// </summary>
        public IState CurrentState
        {
            get => _currentState;
        }

        /// <summary>
        ///     获取当前激活状态的ID（只读）
        /// </summary>
        public TStateId CurrentStateId
        {
            get => _currentStateId;
        }

        /// <summary>
        ///     获取上一个激活状态的ID（只读，用于状态回溯场景）
        /// </summary>
        public TStateId PreviousStateId
        {
            get => _previousStateId;
        }

        /// <summary>
        ///     获取当前状态已持续的帧数（只读，用于帧级精度的状态时长判断）
        /// </summary>
        public long FrameCountOfCurrentState
        {
            get => _frameCountOfCurrentState;
        }

        /// <summary>
        ///     获取当前状态已持续的时间（单位：秒，只读，用于时间级精度的状态时长判断）
        /// </summary>
        public float SecondsOfCurrentState
        {
            get => _secondsOfCurrentState;
        }

    #endregion

    #region 方法

        /// <summary>
        ///     获取或创建指定ID对应的状态（默认创建<see cref="FSMState" />实例）
        /// </summary>
        /// <param name="id">要获取或创建的状态ID</param>
        /// <returns>
        ///     若ID已存在于状态字典中，返回对应的<see cref="FSMState" />实例；
        ///     若ID不存在，创建新的<see cref="FSMState" />实例并添加到字典，再返回该实例
        /// </returns>
        /// <remarks>仅支持创建<see cref="FSMState" />类型的状态，若需自定义状态，建议使用<see cref="AddState" />方法</remarks>
        public FSMState State(TStateId id)
        {
            if (_states.TryGetValue(id, out var value))
            {
                return value as FSMState;
            }

            var state = new FSMState();
            _states.Add(id, state);
            return state;
        }

        /// <summary>
        ///     向状态机注册自定义状态（支持任意实现<see cref="IState" />接口的状态类型）
        /// </summary>
        /// <param name="id">状态的唯一ID（需确保与已有ID不重复，否则会抛出异常）</param>
        /// <param name="state">实现<see cref="IState" />接口的状态实例</param>
        /// <exception cref="ArgumentException">当指定ID已存在于状态字典中时抛出</exception>
        public void AddState(TStateId id, IState state)
        {
            _states.Add(id, state);
        }

        /// <summary>
        ///     切换到指定ID的状态（带条件检查，避免重复切换）
        /// </summary>
        /// <param name="id">目标状态的ID</param>
        /// <remarks>
        ///     状态切换逻辑：
        ///     1. 检查目标ID是否与当前状态ID一致，若一致则跳过切换；
        ///     2. 检查目标状态是否存在于字典中；
        ///     3. 若当前有激活状态，且目标状态满足切换条件（<see cref="IState.Condition" />返回true），则执行切换；
        ///     4. 切换步骤：调用当前状态的<see cref="IState.Exit" /> → 记录上一状态ID → 更新当前状态 → 触发切换回调 → 重置状态时长统计 → 调用目标状态的
        ///     <see cref="IState.Enter" />
        /// </remarks>
        public void ChangeState(TStateId id)
        {
            if (Equals(id, _currentStateId)) return;

            if (_states.TryGetValue(id, out var state))
            {
                if (_currentState != null && state.Condition())
                {
                    _currentState.Exit();
                    _previousStateId = _currentStateId;
                    _currentState    = state;
                    _currentStateId  = id;
                    _onStateChanged?.Invoke(_previousStateId, _currentStateId);
                    _frameCountOfCurrentState = 1;
                    _secondsOfCurrentState    = 0.0f;
                    _currentState.Enter();
                }
            }
        }

        /// <summary>
        ///     注册状态切换时的回调函数
        /// </summary>
        /// <param name="onStateChanged">回调函数：参数1为上一个状态ID，参数2为当前状态ID</param>
        /// <remarks>若多次调用，后续回调会覆盖前一次的回调（仅保留最后一次注册的回调）</remarks>
        public void OnStateChanged(Action<TStateId, TStateId> onStateChanged)
        {
            _onStateChanged = onStateChanged;
        }

        /// <summary>
        ///     启动状态机并进入指定ID的初始状态（无切换条件检查，用于状态机初始化）
        /// </summary>
        /// <param name="id">初始状态的ID</param>
        /// <remarks>
        ///     与<see cref="ChangeState" />的区别：
        ///     1. 无状态切换条件检查，直接进入目标状态；
        ///     2. 上一状态ID会被设为初始状态ID（因无前置状态）；
        ///     3. 状态时长统计会被重置为初始值（帧数0，时间0）
        /// </remarks>
        public void StartState(TStateId id)
        {
            if (_states.TryGetValue(id, out var state))
            {
                _previousStateId          = id;
                _currentState             = state;
                _currentStateId           = id;
                _frameCountOfCurrentState = 0;
                _secondsOfCurrentState    = 0.0f;
                _currentState.Enter();
            }
        }

        /// <summary>
        ///     调用当前状态的固定时间更新逻辑（建议在Unity的FixedUpdate中调用，用于物理相关逻辑）
        /// </summary>
        /// <remarks>若当前无激活状态（_currentState为null），则不执行任何操作</remarks>
        public void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        /// <summary>
        ///     调用当前状态的每帧更新逻辑，并更新状态时长统计（建议在Unity的Update中调用）
        /// </summary>
        /// <remarks>
        ///     执行逻辑：
        ///     1. 调用当前状态的<see cref="IState.Update" />；
        ///     2. 累加当前状态的持续帧数（_frameCountOfCurrentState++）；
        ///     3. 累加当前状态的持续时间（_secondsOfCurrentState += Time.deltaTime）
        /// </remarks>
        public void Update()
        {
            _currentState?.Update();
            _frameCountOfCurrentState++;
            _secondsOfCurrentState += Time.deltaTime;
        }

        /// <summary>
        ///     调用当前状态的GUI绘制逻辑（建议在Unity的OnGUI中调用，用于调试界面绘制）
        /// </summary>
        /// <remarks>若当前无激活状态（_currentState为null），则不执行任何操作</remarks>
        public void OnGUI()
        {
            _currentState?.OnGUI();
        }

        /// <summary>
        ///     清空状态机的所有数据（重置状态、状态ID、状态字典）
        /// </summary>
        /// <remarks>
        ///     清空逻辑：
        ///     1. 置空当前状态实例（_currentState = null）；
        ///     2. 重置当前/上一状态ID为默认值；
        ///     3. 清空状态字典（_states.Clear()）；
        ///     注意：此方法不会调用当前状态的<see cref="IState.Exit" />，若需正常退出状态，建议先调用<see cref="ChangeState" />切换到退出状态
        /// </remarks>
        public void Clear()
        {
            _currentState    = null;
            _currentStateId  = default(TStateId);
            _previousStateId = default(TStateId);
            _states.Clear();
        }

    #endregion
    }
}