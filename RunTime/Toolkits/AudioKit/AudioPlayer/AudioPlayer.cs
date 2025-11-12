// ------------------------------------------------------------
// @file       AudioPlayer.cs
// @brief
// @author     zheliku
// @Modified   2024-11-14 11:11:21
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    using Core;
    using PoolKit;
    using TimerKit;
    using UnityEngine;
    using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class AudioPlayer : IPoolable, IPoolType
    {
    #region Static

        internal static AudioPlayer Create(BindableProperty<float> volume)
        {
            var player = SingletonPool<AudioPlayer>.Get();
            player.Volume = volume; // 指向 AudioKitSetting 中的 Volume
            player.playEvent = new AudioPlayerEvent(player)
            {
                onStart  = null,
                onFinish = null
            };
            player.settingVolume = 1;
            player.volumeScale   = 1;

            // 创建时，为 AudioKitSetting 中的 Volume 绑定事件：更改值时，同步更改 AudioSource 的音量
            player.Volume.RegisterWithInitValue(player.OnAudioSettingVolumeChanged);

            return player;
        }

    #endregion

    #region 字段

        private IAudioLoader _loader;

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private Timer _timer;

        internal AudioPlayerEvent playEvent;

        private  bool  _isPaused;
        internal float volumeScale = 1;
        internal float settingVolume; // AudioKitSetting 中的 Volume，实际音量通过 _volumeScale 和 _settingVolume 共同控制

    #endregion

    #region 属性

        public BindableProperty<float> Volume { get; set; } // 指向 AudioKitSetting 中的 Volume

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public string AudioClipName { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public AudioSource AudioSource { get; private set; }

        public bool IsInPool { get; set; }

        public bool UsedCache { get; set; } = true;

        public bool IsLoop { get; set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public AudioClip AudioClip { get; private set; }

        public bool IsPlaying
        {
            get => AudioSource != null && AudioSource.isPlaying;
        }

        public float PlayedCount { get; private set; } // 已播放次数

    #endregion

    #region 公共方法

        /// <summary>
        ///     加载 Clip 并播放
        /// </summary>
        /// <param name="clipName">clip 名称</param>
        /// <param name="loop">是否循环播放</param>
        /// <param name="attachedObject">将 AudioSource 挂载到哪个 GameObject 上</param>
        /// <returns>自己</returns>
        public AudioPlayer PlayAsync(string clipName, bool loop, GameObject attachedObject = null)
        {
            // 加载同名 Clip，则跳过
            if (string.IsNullOrEmpty(clipName) || AudioClipName == clipName)
            {
                return this;
            }

            CheckAudioSource(attachedObject);

            // 防止卸载后立马加载的情况
            var preLoader = _loader;
            _loader = null;
            ClearResources();

            _loader = AudioKit.AudioLoaderPool.Get();

            // 记录设置
            IsLoop        = loop;
            AudioClipName = clipName;

            _loader.LoadClipAsync(AudioClipName, (success, clip) =>
            {
                if (!success)
                {
                    Release();
                    return;
                }

                // 记录 clip
                AudioClip = clip;

                if (AudioClip == null)
                {
                    Debug.LogError("Asset Is Invalid AudioClip: " + AudioClipName);
                    Release();
                    return;
                }

                PlayInternal(); // 依据记录信息进行播放
            });

            if (preLoader != null)
            {
                preLoader.Unload();
                AudioKit.AudioLoaderPool.Release(preLoader);
            }

            return this;
        }

        /// <summary>
        ///     播放已有 AudioClip
        /// </summary>
        /// <param name="clip">音频</param>
        /// <param name="loop">是否循环</param>
        /// <param name="attachedObject">将 AudioSource 挂载到哪个 GameObject 上</param>
        public void PlayClip(AudioClip clip, bool loop, GameObject attachedObject = null)
        {
            if (clip == null)
            {
                return;
            }

            CheckAudioSource(attachedObject);

            ClearResources();

            IsLoop        = loop;
            AudioClipName = clip.name;
            AudioClip     = clip;

            PlayInternal();
        }

        public void Stop()
        {
            Release();
        }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public void Pause()
        {
            if (_isPaused)
            {
                return;
            }

            if (_timer != null)
            {
                _timer.Paused = true;
            }

            _isPaused = true;

            AudioSource.Pause();
        }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public void UnPause()
        {
            if (!_isPaused)
            {
                return;
            }

            _timer.Paused = false;
            _isPaused     = false;

            AudioSource.Play(); // 与 UnPause 方法相同
        }

        public void OnCreate() { }

        public void OnGet() { }

        public void OnRelease()
        {
            Volume?.Unregister(OnAudioSettingVolumeChanged);
            Volume = null;

            playEvent.onStart  = null;
            playEvent.onFinish = null;
            PlayedCount        = 0;

            ClearResources();
        }

        public void OnDestroy() { }

        public void Recycle2Pool()
        {
            if (!SingletonPool<AudioPlayer>.Release(this))
            {
                // 如果无法回收，则直接销毁 AudioSource
                if (AudioSource)
                {
                    Object.Destroy(AudioSource);
                    AudioSource = null;
                }
            }
        }

    #endregion

    #region 其他方法

        private void CheckAudioSource(GameObject attachedObject)
        {
            if (AudioSource == null)
            {
                if (attachedObject != null)
                {
                    AudioSource = attachedObject.AddComponent<AudioSource>();
                }
                else
                {
                    AudioSource = AudioMgr.Instance.gameObject.AddComponent<AudioSource>();
                }
            }
        }

        private void ClearResources()
        {
            AudioClipName = null;
            _isPaused     = false;

            if (_timer != null) // 回收 _timer
            {
                _timer.Cancel();
                _timer.Recycle2Pool();
                _timer = null;
            }

            if (AudioSource) // 不回收 _audioSource，重复利用
            {
                if (AudioSource.clip == AudioClip)
                {
                    AudioSource.Stop();
                    AudioSource.clip = null;
                }
            }

            AudioClip = null;

            if (_loader != null)
            {
                _loader.Unload();
                AudioKit.AudioLoaderPool.Release(_loader);
                _loader = null;
            }
        }

        private void Release()
        {
            ClearResources();

            if (UsedCache)
            {
                Recycle2Pool();
            }
        }

        /// <summary>
        ///     事件：更改值时，同步更改 AudioSource 的音量
        /// </summary>
        /// <param name="oldValue">更改前的音量</param>
        /// <param name="volume">更改后的音量</param>
        private void OnAudioSettingVolumeChanged(float oldValue, float volume)
        {
            settingVolume = volume;

            UpdateAudioSourceVolume();
        }

        /// <summary>
        ///     同步更改 AudioSource 的音量
        /// </summary>
        internal void UpdateAudioSourceVolume()
        {
            if (AudioSource)
            {
                AudioSource.volume = volumeScale * settingVolume;
            }
        }

        /// <summary>
        ///     依据记录信息进行播放
        /// </summary>
        private void PlayInternal()
        {
            if (!AudioSource || !AudioClip)
            {
                Release();
                return;
            }

            AudioSource.clip = AudioClip;
            AudioSource.loop = IsLoop;
            UpdateAudioSourceVolume();

            _timer = TimerKit.CreateScaled(OnAudioClipPlayFinish, AudioClip.length, IsLoop ? -1 : 1);

            playEvent.CallOnStart();
            // _onStart?.Invoke(this);

            AudioSource.Play();
        }

        /// <summary>
        ///     事件：播放结束时执行的方法
        /// </summary>
        /// <param name="timer">依附的计时器</param>
        private void OnAudioClipPlayFinish(Timer timer)
        {
            PlayedCount++;

            playEvent.CallOnFinish();
            // _onFinish?.Invoke(this);

            if (!IsLoop)
            {
                Release();
            }
        }

    #endregion
    }
}