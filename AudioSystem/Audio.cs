using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace FNAF1_Recreation.AudioSystem
{
    public class Audio
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
            foreach (AudioEffect sfx in activesfx)
            {
                sfx.Stop();
            }
        }

        public static void RemoveStoppedSoundsInQueue() =>
            activesfx = activesfx
                .Where(ch => ch.sfxInstance.State != SoundState.Stopped)
                .ToList();
    }
}
