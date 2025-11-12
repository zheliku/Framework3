// ------------------------------------------------------------
// @file       AudioPlayerEvent.cs
// @brief
// @author     zheliku
// @Modified   2025-09-28 17:52:21
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
    using System;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    internal class AudioPlayerEvent
    {
        private readonly AudioPlayer _owner;

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        internal Action<AudioPlayer> onFinish; // 播放结束的回调

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        internal Action<AudioPlayer> onStart; // 开始播放的回调
        public AudioPlayerEvent(AudioPlayer owner)
        {
            _owner = owner;
        }

        internal void RegisterOnStart(Action<AudioPlayer> startEvent)
        {
            if (startEvent == null) return;

            if (onStart == null)
            {
                onStart = startEvent;
            }
            else
            {
                onStart += startEvent;
            }
        }

        internal void RegisterOnFinish(Action<AudioPlayer> finishEvent)
        {
            if (finishEvent == null) return;

            if (onFinish == null)
            {
                onFinish = finishEvent;
            }
            else
            {
                onFinish += finishEvent;
            }
        }

        internal void CallOnStart()
        {
            onStart?.Invoke(_owner);
        }

        internal void CallOnFinish()
        {
            onFinish?.Invoke(_owner);
        }

        internal void Clear()
        {
            onStart  = null;
            onFinish = null;
        }
    }
}