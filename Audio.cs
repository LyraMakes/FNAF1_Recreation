using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework.Audio;

namespace FNAF1_Recreation
{
    class Audio
    {
        public static List<AudioChannel> channels;

        public static AudioChannel Play(SoundEffect sfx)
        {
            channels.Add(new AudioChannel(sfx));
            channels.Last().Play();

            return channels.Last();
        }

        public static AudioChannel Play(SoundEffect sfx, bool isLooping)
        {
            channels.Add(new AudioChannel(sfx));
            channels.Last().Play(isLooping);
            return channels.Last();
        }

        public static void Add(AudioChannel ch) => channels.Add(ch);

        public static void StopAll()
        {
            foreach (AudioChannel c in channels)
            {
                c.Stop();
            }
        }

        public static void Clean()
        {
            List<AudioChannel> sfxChannels = new List<AudioChannel>();

            foreach (AudioChannel ch in channels)
            {
                if (ch.sfxInstance.State != SoundState.Stopped)
                {
                    sfxChannels.Add(ch);
                }
            }

            channels = sfxChannels;

        }
    }

    class AudioChannel
    {
        public SoundEffectInstance sfxInstance;

        public AudioChannel(SoundEffect sfx)
        {
            sfxInstance = sfx.CreateInstance();
        }


        public void Play() => sfxInstance.Play();

        public void Play(bool looping)
        {
            sfxInstance.IsLooped = looping;
            sfxInstance.Play();
        }

        public void Stop() => sfxInstance.Stop();
    }
}
