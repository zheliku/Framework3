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
    using ResKit;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AddressablesAudioLoader : IAudioLoader
    {
        public AsyncOperationHandle<AudioClip> Handle { get; private set; }

        public AudioClip Clip { get => Handle.Result; }

        public AudioClip LoadClip(string clipName)
        {
            // 默认通过 ResKit 加载音频
            Handle = Addressables.LoadAssetAsync<AudioClip>(clipName);
            Handle.WaitForCompletion();
            return Handle.Result;
        }

        public void LoadClipAsync(string clipName, Action<bool, AudioClip> onLoad = null)
        {
            Handle = Addressables.LoadAssetAsync<AudioClip>(clipName);
            Handle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // 加载成功，但还需要检查结果是否为null
                    if (handle.Result != null)
                    {
                        onLoad?.Invoke(true, handle.Result);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load audio clip: {clipName}. Asset is null despite successful operation.");
                        onLoad?.Invoke(false, null);
                    }
                }
                else if (handle.Status == AsyncOperationStatus.Failed)
                {
                    // 加载失败，记录详细错误信息
                    Debug.LogError($"Failed to load audio clip: {clipName}. Error: {handle.OperationException?.Message}");
                    onLoad?.Invoke(false, null);
                }
                else
                {
                    // 其他状态（如取消）
                    Debug.LogWarning($"Audio clip loading was not completed: {clipName}. Status: {handle.Status}");
                    onLoad?.Invoke(false, null);
                }
            };
        }

        public void Unload()
        {
            Handle.Unload();
        }
    }
}