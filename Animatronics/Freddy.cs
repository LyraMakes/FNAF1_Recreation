using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class Freddy : Animatronic
    {
        public int movementCountdown;

        public Room CurrentRoom
        {
            get
            {
                return path.rooms[path.roomIndex];
            }
            set
            {
                path.roomIndex = path.rooms.FindIndex(r => r.Equals(value));
            }
        }

        private readonly AnimatronicPath path;

        public Freddy(Rand rand) : base(rand)
        {
            movementOffset = 3.02;

            path = new AnimatronicPath();

            path.AddRoom(Room.rooms[1]);
            path.AddRoom(Room.rooms[10]);
            path.AddRoom(Room.rooms[9]);
            path.AddRoom(Room.rooms[6]);
            path.AddRoom(Room.rooms[7]);
        }

        public void TryMove()
        {
            if (MovementOpportunity()) OnSuccessfulMovement();
        }

        private void OnSuccessfulMovement()
        {
            CurrentRoom = path.NextRoom();
        }
    }
}
