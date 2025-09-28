// ------------------------------------------------------------
// @file       AudioPlayerExtension.cs
// @brief
// @author     zheliku
// @Modified   2025-09-28 18:13:53
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.AudioKit
{
    using System;

    public static class AudioPlayerExtension
    {
        public static AudioPlayer OnStart(this AudioPlayer player, Action<AudioPlayer> onStart)
        {
            player.playEvent.RegisterOnStart(onStart);
            return player;
        }

        public static AudioPlayer OnFinish(this AudioPlayer player, Action<AudioPlayer> onFinish)
        {
            player.playEvent.RegisterOnFinish(onFinish);
            return player;
        }
        
        public static AudioPlayer VolumeScale(this AudioPlayer player, float volumeScale)
        {
            player.volumeScale = volumeScale;
            player.UpdateAudioSourceVolume();
            return player;
        }
    }
}