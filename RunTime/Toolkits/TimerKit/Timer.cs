// ------------------------------------------------------------
// @file       AudioTimer.cs
// @brief      定义计时器类及其相关功能，用于管理计时器的创建、触发和回收。
// @author     zheliku
// @Modified   2024-11-14 15:11:52
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.TimerKit
{
    using System;
    using PoolKit;
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// 定义计时器类型，支持缩放时间和非缩放时间。
    /// </summary>
    public enum TimerType
    {
        Scaled,  // 缩放时间
        Unscaled // 非缩放时间
    }

    /// <summary>
    /// Timer 类用于管理计时器的生命周期，包括创建、触发、暂停、取消和回收。
    /// </summary>
    [HideReferenceObjectPicker]
    public class Timer : IPoolable, IPoolType
    {
            #region Static

        /// <summary>
        /// 创建一个新的计时器实例。
        /// </summary>
        /// <param name="onTick">计时器触发时的回调函数。</param>
        /// <param name="duration">计时器的持续时间（秒）。</param>
        /// <param name="repeatCount">计时器的重复次数，默认为 1。</param>
        /// <param name="timerType">计时器的时间类型（缩放或非缩放），默认为 TimerType.Scaled。</param>
        /// <returns>返回创建的计时器实例。</returns>
        public static Timer Spawn(Action<Timer> onTick, float duration, int repeatCount = 1, TimerType timerType = TimerType.Scaled)
        {
            var timer = SingletonPool<Timer>.Get();
            timer.Enabled       = true;
            timer.TickCount     = 0;
            timer._onTickAction = onTick;
            timer.DelayTime     = duration;
            timer.RepeatCount   = repeatCount;
            timer.TimerType     = timerType;
            timer.CreateTime    = timer.CurrentTime;
            timer.LastTickTime  = timer.CurrentTime;

            return timer;
        }

            #endregion

            #region 字段

        [ShowInInspector]
        private Action<Timer> _onTickAction; // Timer 的触发事件

        private bool _paused;

        // [ShowInInspector]
        private float _pausedProgress;

            #endregion

            #region 属性

        /// <summary>
        /// 获取或设置计时器是否启用。
        /// </summary>
        [ShowInInspector]
        public bool Enabled { get; private set; } = true;

        /// <summary>
        /// 获取或设置计时器是否在对象池中。
        /// </summary>
        // [ShowInInspector]
        public bool IsInPool { get; set; }

        /// <summary>
        /// 获取或设置计时器的延迟时间（秒）。
        /// </summary>
        [ShowInInspector]
        public float DelayTime { get; set; }

        /// <summary>
        /// 获取计时器的创建时间。
        /// </summary>
        // [ShowInInspector]
        public float CreateTime { get; private set; }

        /// <summary>
        /// 获取计时器上一次触发的时间。
        /// </summary>
        // [ShowInInspector]
        [ShowInInspector]
        public float LastTickTime { get; private set; }

        /// <summary>
        /// 获取计时器下一次触发的时间。
        /// </summary>
        // [ShowInInspector]
        [ShowInInspector]
        public float TargetTime { get => LastTickTime + DelayTime; }

        /// <summary>
        /// 获取计时器的当前时间，基于 TimerType。
        /// </summary>
        [ShowInInspector]
        [ProgressBar(nameof(LastTickTime), nameof(TargetTime))]
        public float CurrentTime
        {
            get => TimerType == TimerType.Scaled
                       ? Time.time
                       : Time.unscaledTime;
        }

        /// <summary>
        /// 获取计时器的类型（缩放或非缩放）。
        /// </summary>
        [ShowInInspector]
        public TimerType TimerType { get; private set; }

        /// <summary>
        /// 获取计时器是否为循环计时器。
        /// </summary>
        [ShowInInspector]
        public bool Loop { get => RepeatCount < 0; }

        /// <summary>
        /// 获取或设置计时器是否暂停。
        /// </summary>
        [ShowInInspector]
        public bool Paused
        {
            get => _paused;
            set
            {
                if (value)
                {
                    _pausedProgress = CurrentTime - LastTickTime;
                }
                else
                {
                    LastTickTime = CurrentTime - _pausedProgress;
                }
            }
        }

        /// <summary>
        /// 获取计时器已触发的次数。
        /// </summary>
        [ShowInInspector]
        public int TickCount { get; private set; }

        /// <summary>
        /// 获取计时器的循环次数。
        /// 小于 0 表示无限循环；等于 0 表示被回收；大于 0 表示具体的循环次数。
        /// </summary>
        [ShowInInspector]
        public int RepeatCount { get; private set; }

            #endregion

            #region 公共方法

        /// <summary>
        /// 触发计时器的回调函数。
        /// </summary>
        public void Tick()
        {
            ++TickCount;
            _onTickAction?.Invoke(this);
        }

        /// <summary>
        /// 取消计时器。
        /// </summary>
        public void Cancel()
        {
            if (Enabled)
            {
                Enabled       = false;
                _onTickAction = null;
            }
        }

        /// <summary>
        /// 尝试重复计时器，如果可以重复则更新 LastTickTime。
        /// </summary>
        /// <returns>如果可以重复，返回 true；否则返回 false。</returns>
        public bool TryRepeat()
        {
            if (RepeatCount < 0 || TickCount < RepeatCount)
            {
                LastTickTime += DelayTime;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 当计时器被创建时调用。
        /// </summary>
        public void OnCreate() { }

        /// <summary>
        /// 当计时器从对象池中获取时调用。
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// 当计时器被释放时调用，重置计时器的状态。
        /// </summary>
        public void OnRelease()
        {
            _onTickAction = null;
            DelayTime     = 0;
            RepeatCount   = 0;
            TickCount     = 0;
            Enabled       = false;
        }

        /// <summary>
        /// 当计时器被销毁时调用。
        /// </summary>
        public void OnDestroy() { }

        /// <summary>
        /// 将计时器回收到对象池中。
        /// </summary>
        public void Recycle2Pool()
        {
            SingletonPool<Timer>.Release(this);
        }

            #endregion
    }
}