// ------------------------------------------------------------
// @file       AudioManager.cs
// @brief
// @author     zheliku
// @Modified   2024-11-14 17:11:27
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using SingletonKit;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

#if UNITY_EDITOR
#endif

    [MonoSingletonPath("Framework3/AudioKit/AudioMgr")]
    public class AudioMgr : MonoSingleton<AudioMgr>
    {
    #region Static

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private static Dictionary<string, List<AudioPlayer>> s_soundPlayerInPlaying = new(30);

    #endregion

    #region 字段

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private AudioListener _audioListener;

    #endregion

    #region 属性

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public AudioPlayer MusicPlayer { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public AudioPlayer NarrationPlayer { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public AudioClip CurrentMusic { get; set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public AudioClip CurrentNarration { get; set; }

    #endregion

    #region 公共方法

        public override void OnSingletonInit()
        {
            // 与 AudioKit.Setting.MusicVolume 绑定
            MusicPlayer           = AudioPlayer.Create(AudioKit.Setting.MusicVolume);
            MusicPlayer.UsedCache = false;
            MusicPlayer.IsLoop    = true;

            // 与 AudioKit.Setting.NarrationVolume 绑定
            NarrationPlayer           = AudioPlayer.Create(AudioKit.Setting.NarrationVolume);
            NarrationPlayer.UsedCache = false;
            NarrationPlayer.IsLoop    = false;

            CheckAudioListener();

            AudioKit.Setting.IsMusicOn.Register((oldValue, musicOn) =>
            {
                // 更改 IsMusicOn 即可控制 Music 播放
                if (musicOn)
                {
                    if (CurrentMusic)
                    {
                        AudioKit.PlayMusic(CurrentMusic);
                    }
                }
                else
                {
                    MusicPlayer.Stop();
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);

            AudioKit.Setting.IsNarrationOn.Register((oldValue, narrationOn) =>
            {
                // 更改 IsNarrationOn 即可控制 Narration 播放
                if (narrationOn)
                {
                    if (CurrentNarration)
                    {
                        AudioKit.PlayNarration(CurrentNarration);
                    }
                }
                else
                {
                    NarrationPlayer.Stop();
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);

            AudioKit.Setting.IsSoundOn.Register((oldValue, soundOn) =>
            {
                // 更改 IsSoundOn 即可控制 Sound 播放，打开时不播放已停止的音效
                if (soundOn)
                { }
                else
                {
                    ForEachSound(soundPlayer => soundPlayer.Stop());
                }
            }).UnregisterWhenGameObjectDestroyed(gameObject);
        }

        /// <summary>
        ///     确认 AudioListener 存在
        /// </summary>
        public void CheckAudioListener()
        {
            _audioListener ??= FindFirstObjectByType<AudioListener>();

            _audioListener ??= gameObject.AddComponent<AudioListener>();
        }

        /// <summary>
        ///     遍历所有 Sound，执行 operation
        /// </summary>
        /// <param name="operation">每个 Sound 执行的方法</param>
        public void ForEachSound(Action<AudioPlayer> operation)
        {
            foreach (var audioPlayer in s_soundPlayerInPlaying.SelectMany(keyValuePair => keyValuePair.Value))
            {
                operation(audioPlayer);
            }
        }

        public void AddSoundPlayerToPool(AudioPlayer audioPlayer)
        {
            if (s_soundPlayerInPlaying.ContainsKey(audioPlayer.AudioClipName))
            {
                s_soundPlayerInPlaying[audioPlayer.AudioClipName].Add(audioPlayer);
            }
            else
            {
                s_soundPlayerInPlaying.Add(audioPlayer.AudioClipName, new List<AudioPlayer> { audioPlayer });
            }
        }

        public void RemoveSoundPlayerFromPool(AudioPlayer audioPlayer)
        {
            s_soundPlayerInPlaying[audioPlayer.AudioClipName].Remove(audioPlayer);
        }

        public void ClearAllPlayingSound()
        {
            s_soundPlayerInPlaying.Clear();
        }

    #endregion
    }
}