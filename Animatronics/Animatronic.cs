using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation
{
    class Animatronic
    {
        public string name;
        public int id;

        private readonly IAnimatronicBehavior behavior;
        private AnimatronicLevel AILevel;
        private readonly Rand rand;

        public double movementOffset;

        public Animatronic(string n, int i, IAnimatronicBehavior b, Rand r)
        {
            name = n;
            id = i;
            behavior = b;
            AILevel = 0;
            rand = r;
        }

        public Animatronic SetMovemetOffset(double offset)
        {
            movementOffset = offset;
            return this;
        }

        public Animatronic SetAILevel(int l)
        {
            AILevel = l;
            return this;
        }

        //Run Per tick
        public void OnTick() => behavior.OnTick();

        public void OnHour(int hour) => AILevel += (behavior.OnHour(hour)) ? 1 : 0;

        public bool MovementOpportunity() => rand.Next() <= AILevel;
    }
}
