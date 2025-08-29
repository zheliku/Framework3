// ------------------------------------------------------------
// @file       AddressablesAudioLoader.cs
// @brief
// @author     zheliku
// @Modified   2024-11-14 10:11:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
    using System;
    using UnityEngine;

    public class ResourcesAudioLoader : IAudioLoader
    {
        public AudioClip Clip { get; private set; }

        public AudioClip LoadClip(string clipName)
        {
            Clip = Resources.Load<AudioClip>(clipName);
            return Clip;
        }

        public void LoadClipAsync(string clipName, Action<bool, AudioClip> onLoad = null)
        {
            var req = Resources.LoadAsync<AudioClip>(clipName);
            
            req.completed += operation =>
            {
                // 检查加载是否成功
                if (req.asset == null)
                {
                    // 加载失败，记录错误日志
                    Debug.LogError($"Failed to load audio clip: {clipName}. File not found or invalid format.");
                    Clip = null;
                    onLoad?.Invoke(false, null);
                }
                else
                {
                    // 加载成功
                    Clip = req.asset as AudioClip;
                    if (Clip == null)
                    {
                        // 类型转换失败
                        Debug.LogError($"Loaded asset is not an AudioClip: {clipName}");
                        onLoad?.Invoke(false, null);
                    }
                    else
                    {
                        // 加载和转换都成功
                        onLoad?.Invoke(true, Clip);
                    }
                }
            };
        }

        public void Unload()
        {
            if (Clip)
            {
                Resources.UnloadAsset(Clip);
                Clip = null;
            }
        }
    }
}