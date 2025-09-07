// ------------------------------------------------------------
// @file       TimerKit.cs
// @brief      提供创建和管理计时器的工具类。
// @author     zheliku
// @Modified   2024-11-14 16:11:34
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.TimerKit
{
    using System;
    using UnityEngine;

    /// <summary>
    /// TimerKit 提供用于创建和管理计时器的静态方法。
    /// </summary>
    public static class TimerKit
    {
        /// <summary>
        /// 创建一个基于缩放时间的计时器。
        /// </summary>
        /// <param name="onTick">计时器触发时的回调函数。</param>
        /// <param name="duration">计时器的持续时间（秒）。</param>
        /// <param name="repeat">计时器重复的次数，默认为 1。</param>
        /// <returns>返回创建的计时器实例。</returns>
        public static Timer CreateScaled(Action<Timer> onTick, float duration, int repeat = 1)
        {
            return TimerMgr.Instance.CreateTimer(onTick, duration, repeat, TimerType.Scaled);
        }

        /// <summary>
        /// 创建一个基于非缩放时间的计时器。
        /// </summary>
        /// <param name="onTick">计时器触发时的回调函数。</param>
        /// <param name="duration">计时器的持续时间（秒）。</param>
        /// <param name="repeat">计时器重复的次数，默认为 1。</param>
        /// <returns>返回创建的计时器实例。</returns>
        public static Timer CreateUnscaled(Action<Timer> onTick, float duration, int repeat = 1)
        {
            return TimerMgr.Instance.CreateTimer(onTick, duration, repeat, TimerType.Unscaled);
        }

        /// <summary>
        /// 创建一个计时器，可以指定时间类型（缩放或非缩放）。
        /// </summary>
        /// <param name="onTick">计时器触发时的回调函数。</param>
        /// <param name="duration">计时器的持续时间（秒）。</param>
        /// <param name="repeat">计时器重复的次数，默认为 1。</param>
        /// <param name="timerType">计时器的时间类型（缩放或非缩放），默认为 TimerType.Scaled。</param>
        /// <returns>返回创建的计时器实例。</returns>
        public static Timer Create(Action<Timer> onTick, float duration, int repeat = 1, TimerType timerType = TimerType.Scaled)
        {
            return TimerMgr.Instance.CreateTimer(onTick, duration, repeat, timerType);
        }

        /// <summary>
        /// 检查指定对象是否已超过给定的时间间隔。
        /// </summary>
        /// <param name="id">用于标识的对象。</param>
        /// <param name="interval">时间间隔（秒）。</param>
        /// <returns>如果超过时间间隔，返回 true；否则返回 false。</returns>
        public static bool PassIntervalTime(object id, float interval)
        {
            var timeDict = TimerMgr.Instance.TimeDict;
            if (timeDict.TryGetValue(id, out var time))
            {
                if (time + interval <= Time.time)
                {
                    var passedCount = (int) ((Time.time - time) / interval);
                    timeDict[id] += passedCount * interval;
                    return true;
                }
                return false;
            }

            timeDict[id] = Time.time;
            return false;
        }

        /// <summary>
        /// 检查指定对象是否已超过给定的帧间隔。
        /// </summary>
        /// <param name="id">用于标识的对象。</param>
        /// <param name="interval">帧间隔。</param>
        /// <returns>如果超过帧间隔，返回 true；否则返回 false。</returns>
        public static bool PassIntervalFrame(object id, int interval)
        {
            var frameDict = TimerMgr.Instance.FrameDict;
            if (frameDict.TryGetValue(id, out var frame))
            {
                if (frame + interval <= Time.frameCount)
                {
                    frameDict[id] = Time.frameCount;
                    return true;
                }
                return false;
            }

            frameDict[id] = Time.frameCount;
            return false;
        }
    }
}