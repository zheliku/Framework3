// ------------------------------------------------------------
// @file       AudioTimer.cs
// @brief
// @author     zheliku
// @Modified   2024-11-14 14:11:30
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.TimerKit
{
    using System;
    using System.Collections.Generic;
    using PoolKit;
    using SingletonKit;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [MonoSingletonPath("Framework3/TimerKit/TimerMgr")]
    public class TimerMgr : MonoSingleton<TimerMgr>
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public ObjectPool<Timer> TimerPool { get => SingletonPool<Timer>.Pool; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public Dictionary<object, float> TimeDict { get; } = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public Dictionary<object, float> FrameDict { get; } = new();

    #region Unity 事件

        protected override void Update()
        {
            base.Update();

            lock (_lock)
            {
                _timers.RemoveAll(timer =>
                {
                    if (!timer.Enabled)
                    {
                        if (!timer.IsInPool) timer.Recycle2Pool();
                        return true;
                    }

                    if (!timer.Paused && timer.TargetTime <= timer.CurrentTime)
                    {
                        timer.Tick();
                        if (!timer.TryRepeat())
                        {
                            if (!timer.IsInPool) timer.Recycle2Pool();
                            return true;
                        }
                    }

                    return false;
                });
            }
        }

    #endregion

    #region 公共方法

        public Timer CreateTimer(Action<Timer> onTick, float duration, int repeat = 1,
                                 TimerType     timerType = TimerType.Scaled)
        {
            lock (_lock)
            {
                var timer = Timer.Spawn(onTick, duration, repeat, timerType);
                _timers.Add(timer);
                return timer;
            }
        }

    #endregion

    #region 字段

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly List<Timer> _timers = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public float ScaleTime
        {
            get => Time.time;
        }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public float UnScaledTime
        {
            get => Time.unscaledTime;
        }

        private readonly object _lock = new();

    #endregion
    }
}