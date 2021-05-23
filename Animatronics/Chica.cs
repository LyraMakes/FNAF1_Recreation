using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation.Animatronics
{
    class Chica : Animatronic
    {
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

        public Chica(Rand rand) : base(rand)
        {
            movementOffset = 4.98;

            path = new AnimatronicPath();

            path.AddRoom(Room.rooms[1]);
            path.AddRoom(Room.rooms[10]);
            path.AddRoom(Room.rooms[9]);
            path.AddRoom(Room.rooms[6]);
            path.AddRoom(Room.rooms[7]);
            path.AddRoom(Room.rooms[12]);
        }

        public void TryMove()
        {
            if (MovementOpportunity()) OnSuccessfulMovement();
        }

        private void OnSuccessfulMovement()
        {
            CurrentRoom = path.Random(CurrentRoom);
        }
    }
}
