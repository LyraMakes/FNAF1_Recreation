using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class FoxyBehavior : IAnimatronicBehavior
    {
        public int phase;

        public FoxyBehavior()
        {
            phase = 0;
        }

        public void OnTick()
        {
            Console.WriteLine("Foxy Tick");
        }

        public bool OnHour(int hour) => hour == 3 || hour == 4;

        public Room OnMove(Room start) { phase++; return start; }
    }
}
