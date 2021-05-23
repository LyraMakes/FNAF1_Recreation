using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class Bonnie : Animatronic
    {
        public Room CurrentRoom {
            get
            {
                return path.rooms[path.roomIndex];
            }
            set {
                path.roomIndex = path.rooms.FindIndex(r => r.Equals(value));
            }
        }

        private readonly AnimatronicPath path;

        public Bonnie(Rand rand) : base(rand)
        {
            movementOffset = 4.97;

            path = new AnimatronicPath(rand);

            path.AddRoom(Room.rooms[1]);
            path.AddRoom(Room.rooms[3]);
            path.AddRoom(Room.rooms[4]);
            path.AddRoom(Room.rooms[5]);
            path.AddRoom(Room.rooms[8]);
        }

        public void TryMove()
        {
            if (MovementOpportunity()) OnSuccessfulMovement();
        }

        private void OnSuccessfulMovement()
        {
            if (CurrentRoom == Room.rooms[4]) CurrentRoom = Room.rooms[11];
            CurrentRoom = path.Random();
        }
    }
}
