using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class Animatronic
    {
        public string name;

        protected AnimatronicLevel AILevel;
        protected readonly Rand rand;

        public double moveStartTime;
        public double movementOffset;

        public Animatronic(Rand r)
        {
            name = "TEMP";
            AILevel = 0;
            rand = r;
        }

        public void SetAILevel(int l) => AILevel = l;

        public bool MovementOpportunity()
        {
            Debug.WriteLine($"Using {AILevel} / 20 for MO calc");
            return rand.Next() <= AILevel;
        }
    }
}
