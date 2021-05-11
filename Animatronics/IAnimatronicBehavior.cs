using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation
{
    interface IAnimatronicBehavior
    {
        void OnTick();
        bool OnHour(int hour);
        Room OnMove(Room start);
    }
}
