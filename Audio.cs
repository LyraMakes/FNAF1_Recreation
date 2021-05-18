//using System;
using System.Collections.Generic;
//using System.Text;
using System.Linq;

using Microsoft.Xna.Framework.Audio;

namespace FNAF1_Recreation
{
    class Audio
    {
        public static float MasterVol = 1.0f;

        public static List<AudioEffect> activesfx;

        public static AudioEffect Play(SoundEffect sfx, AudioChannel channel)
        {
            activesfx.Add(new AudioEffect(sfx, channel));
            activesfx.Last().Play();

            return activesfx.Last();
        }

        public static AudioEffect Play(SoundEffect sfx, AudioChannel channel, bool isLooping)
        {
            activesfx.Add(new AudioEffect(sfx, channel));
            activesfx.Last().Play(isLooping);
            return activesfx.Last();
        }

        public static void Add(AudioEffect ch) => activesfx.Add(ch);

        public static void StopAll()
        {
            foreach (AudioEffect c in activesfx)
            {
                c.Stop();
            }
        }

        public static void Clean()
        {
            List<AudioEffect> sfxChannels = new List<AudioEffect>();

            foreach (AudioEffect ch in activesfx)
            {
                if (ch.sfxInstance.State != SoundState.Stopped)
                {
                    sfxChannels.Add(ch);
                }
            }

            activesfx = sfxChannels;

        }
    }

    class AudioEffect
    {
        public AudioChannel channel;
        public SoundEffectInstance sfxInstance;

        public AudioEffect(SoundEffect sfx, AudioChannel chn)
        {
            channel = chn;
            sfxInstance = sfx.CreateInstance();
            sfxInstance.Volume = chn.volume * Audio.MasterVol;
        }


        public void Play() => sfxInstance.Play();

        public void Play(bool looping)
        {
            sfxInstance.IsLooped = looping;
            sfxInstance.Play();
        }

        public void Stop() => sfxInstance.Stop();
    }

    class AudioChannel
    {
        public float volume;

        public AudioChannel(float vol) {
            volume = vol;
        }
    }
}
