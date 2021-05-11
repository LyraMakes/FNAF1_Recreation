using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class ChicaBehavior : IAnimatronicBehavior
    {
        private AnimatronicPath path;

        public Room current;

        public ChicaBehavior()
        {
            current = Room.rooms[0];

            path = new AnimatronicPath();

            path.AddRoom(Room.rooms[1]);
            path.AddRoom(Room.rooms[3]);
            path.AddRoom(Room.rooms[4]);
            path.AddRoom(Room.rooms[5]);
            path.AddRoom(Room.rooms[8]);

        }

        public void OnTick()
        {
            Console.WriteLine("Chica Tick");
        }

        public bool OnHour(int hour) => hour == 3 || hour == 4;

        public Room OnMove(Room start) { return path.Random(current); }
    }
}
