// ------------------------------------------------------------
// @file       IState.cs
// @brief      有限状态机（FSM）的状态接口，定义所有状态必须实现的生命周期方法
// @author     zheliku
// @Modified   2024-10-20 20:10:20
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FSMKit
{
    using Sirenix.OdinInspector;

    /// <summary>
    /// 有限状态机（FSM）的核心状态接口，定义了状态生命周期的标准方法
    /// 所有状态类（如<see cref="FSMState"/>、<see cref="AbstractState{TStateId, TOwner}"/>）均需实现此接口
    /// 确保状态机（<see cref="FSM{TStateId}"/>）能统一管理不同状态的生命周期
    /// </summary>
    [HideReferenceObjectPicker] // Odin Inspector特性：隐藏接口实例的引用对象选择器，简化编辑器界面（仅对接口实现类的实例生效）
    public interface IState
    {
        /// <summary>
        /// 状态切换/保持的条件检查方法
        /// </summary>
        /// <returns>
        /// true：满足状态切换到当前状态的条件，或当前状态可继续保持；
        /// false：不满足条件，状态机需考虑切换到其他状态
        /// </returns>
        /// <remarks>
        /// 调用时机：状态机执行<see cref="FSM{TStateId}.ChangeState"/>时，会先调用此方法判断目标状态是否可进入；
        /// 部分实现中也可能在状态更新时调用，用于判断当前状态是否需提前退出
        /// </remarks>
        bool Condition();

        /// <summary>
        /// 状态进入时的初始化方法
        /// </summary>
        /// <remarks>
        /// 调用时机：仅在状态被首次切换进入时调用一次（如<see cref="FSM{TStateId}.StartState"/>启动初始状态、
        /// <see cref="FSM{TStateId}.ChangeState"/>切换到新状态时）；
        /// 典型用途：初始化状态所需的资源（如重置参数、播放动画、开启计时器）
        /// </remarks>
        void Enter();

        /// <summary>
        /// 状态每帧更新的逻辑方法
        /// </summary>
        /// <remarks>
        /// 调用时机：与状态机的<see cref="FSM{TStateId}.Update"/>同步，每帧调用一次（帧率不固定，依赖设备性能）；
        /// 典型用途：处理非物理相关的逻辑（如输入检测、UI更新、状态切换条件判断）
        /// </remarks>
        void Update();

        /// <summary>
        /// 状态固定时间更新的物理逻辑方法
        /// </summary>
        /// <remarks>
        /// 调用时机：与状态机的<see cref="FSM{TStateId}.FixedUpdate"/>同步，固定时间间隔调用一次（默认0.02秒/次，与Unity物理引擎同步）；
        /// 典型用途：处理物理相关逻辑（如碰撞检测、刚体受力、物理运动计算），避免因帧率波动导致逻辑异常
        /// </remarks>
        void FixedUpdate();

        /// <summary>
        /// 状态的GUI绘制与调试方法
        /// </summary>
        /// <remarks>
        /// 调用时机：与状态机的<see cref="FSM{TStateId}.OnGUI"/>同步，每帧调用一次（仅在Unity编辑器中生效）；
        /// 典型用途：绘制状态调试信息（如当前状态名称、持续时长、关键参数值），方便开发阶段调试状态逻辑
        /// </remarks>
        void OnGUI();

        /// <summary>
        /// 状态退出时的清理方法
        /// </summary>
        /// <remarks>
        /// 调用时机：仅在当前状态被切换退出时调用一次（如<see cref="FSM{TStateId}.ChangeState"/>切换到其他状态前）；
        /// 典型用途：清理状态占用的资源（如停止动画、关闭计时器、重置临时变量），避免资源泄漏或状态残留影响
        /// </remarks>
        void Exit();
    }
}