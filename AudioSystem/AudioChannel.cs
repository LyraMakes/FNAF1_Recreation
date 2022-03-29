using System.Collections.Generic;
using System.Security;

namespace FNAF1_Recreation.AudioSystem
{
    public class AudioChannel
    {
        public static Dictionary<string, AudioChannel> Channels;

        public float volume;

        public AudioChannel(float vol) {
            volume = vol;
        }
    }
}