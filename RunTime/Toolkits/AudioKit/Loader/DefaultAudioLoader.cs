// ------------------------------------------------------------
// @file       DefaultAudioLoader.cs
// @brief
// @author     zheliku
// @Modified   2024-11-14 10:11:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
    using System;
    using ResKit;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class DefaultAudioLoader : IAudioLoader
    {
        public AsyncOperationHandle<AudioClip> Handle { get; private set; }

        public AudioClip Clip { get => Handle.Result; }

        public AudioClip LoadClip(string clipName)
        {
            // 默认通过 ResKit 加载音频
            Handle = ResKit.LoadAsset<AudioClip>(clipName);
            return Handle.Result;
        }

        public void LoadClipAsync(string clipName, Action<bool, AudioClip> onLoad = null)
        {
            // 默认通过 ResKit 加载音频
            Handle = ResKit.LoadAssetAsync<AudioClip>(clipName, clip =>
            {
                onLoad?.Invoke(true, clip);
            });
        }

        public void Unload()
        {
            Handle.Unload();
        }
    }
}