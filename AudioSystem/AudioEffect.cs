using Microsoft.Xna.Framework.Audio;

namespace FNAF1_Recreation.AudioSystem
{
    public class AudioEffect
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
}