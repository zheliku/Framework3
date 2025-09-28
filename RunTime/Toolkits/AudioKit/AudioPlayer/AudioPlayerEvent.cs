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
    using Sirenix.OdinInspector;

    internal class AudioPlayerEvent
    {
        public AudioPlayerEvent(AudioPlayer owner)
        {
            _owner = owner;
        }

        private AudioPlayer _owner;
        
        [ShowInInspector]
        internal Action<AudioPlayer> onStart; // 开始播放的回调

        [ShowInInspector]
        internal Action<AudioPlayer> onFinish; // 播放结束的回调

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
            onStart = null;
            onFinish = null;
        }
    }
}