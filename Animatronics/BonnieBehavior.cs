using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class BonnieBehavior : IAnimatronicBehavior
    {
        private readonly AnimatronicPath path;

        public BonnieBehavior(Rand r)
        {
            path = new AnimatronicPath(r);

            path.AddRoom(Room.rooms[1]);
            path.AddRoom(Room.rooms[3]);
            path.AddRoom(Room.rooms[4]);
            path.AddRoom(Room.rooms[5]);
            path.AddRoom(Room.rooms[8]);

        }
        
        public void OnTick()
        {
            Console.WriteLine("Bonnie Tick");
        }

        public bool OnHour(int hour) => hour == 2 || hour == 3 || hour == 4;

        public Room OnMove(Room start) => path.Random();
    }
}
