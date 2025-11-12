// ------------------------------------------------------------
// @file       AudioKitSetting.cs
// @brief
// @author     zheliku
// @Modified   2024-11-14 09:11:29
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
    using BindableKit;
    using SingletonKit;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    [MonoSingletonPath("Framework3/AudioKit/AudioKitSetting")]
    public class AudioKitSetting : MonoSingleton<AudioKitSetting>
    {
        // 用来存储的 Key
        private const string KEY_AUDIO_MANAGER_SOUND_ON     = "KEY_AUDIO_MANAGER_SOUND_ON";
        private const string KEY_AUDIO_MANAGER_MUSIC_ON     = "KEY_AUDIO_MANAGER_MUSIC_ON";
        private const string KEY_AUDIO_MANAGER_NARRATION_ON = "KEY_AUDIO_MANAGER_NARRATION_ON";

        private const string KEY_AUDIO_MANAGER_NARRATION_VOLUME = "KEY_AUDIO_MANAGER_NARRATION_VOLUME";
        private const string KEY_AUDIO_MANAGER_SOUND_VOLUME     = "KEY_AUDIO_MANAGER_SOUND_VOLUME";
        private const string KEY_AUDIO_MANAGER_MUSIC_VOLUME     = "KEY_AUDIO_MANAGER_MUSIC_VOLUME";

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public PlayerPrefsBoolProperty IsSoundOn { get; private set; } // 数据直接存储在 PlayerPrefs 中

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public PlayerPrefsBoolProperty IsMusicOn { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public PlayerPrefsBoolProperty IsNarrationOn { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public PlayerPrefsFloatProperty SoundVolume { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public PlayerPrefsFloatProperty MusicVolume { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public PlayerPrefsFloatProperty NarrationVolume { get; private set; }

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public bool IsOn
        {
            get => IsMusicOn && IsSoundOn && IsNarrationOn;
            set
            {
                IsMusicOn.Value     = value;
                IsSoundOn.Value     = value;
                IsNarrationOn.Value = value;
            }
        }

        public override void OnSingletonInit()
        {
            IsSoundOn = new PlayerPrefsBoolProperty(KEY_AUDIO_MANAGER_SOUND_ON, true);

            IsMusicOn = new PlayerPrefsBoolProperty(KEY_AUDIO_MANAGER_MUSIC_ON, true);

            IsNarrationOn = new PlayerPrefsBoolProperty(KEY_AUDIO_MANAGER_NARRATION_ON, true);

            SoundVolume = new PlayerPrefsFloatProperty(KEY_AUDIO_MANAGER_SOUND_VOLUME, 0.6f);

            MusicVolume = new PlayerPrefsFloatProperty(KEY_AUDIO_MANAGER_MUSIC_VOLUME, 0.6f);

            NarrationVolume = new PlayerPrefsFloatProperty(KEY_AUDIO_MANAGER_NARRATION_VOLUME, 0.6f);
        }
    }
}