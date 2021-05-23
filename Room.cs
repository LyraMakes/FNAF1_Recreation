using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FNAF1_Recreation.Animatronics;

namespace FNAF1_Recreation
{
    class Room
    {
        public static Room[] rooms;

        public string name;
        public Texture2D roomTex;
        public Texture2D[] roomTexMap;

        public Room(string n) => name = n;

        public static void InitRooms()
        {
            rooms = new Room[15];

            rooms[0] = new Room("Show Stage");
            rooms[1] = new Room("Dining Area");
            rooms[2] = new Room("Pirate Cove");
            rooms[3] = new Room("West Hall");
            rooms[4] = new Room("W. Hall Corner");
            rooms[5] = new Room("Supply Closet");
            rooms[6] = new Room("East Hall");
            rooms[7] = new Room("E. Hall Corner");
            rooms[8] = new Room("Backstage");
            rooms[9] = new Room("Kitchen");
            rooms[10] = new Room("Restrooms");

            //Non-Visible
            rooms[11] = new Room("Left Door");
            rooms[12] = new Room("Right Door");
            rooms[13] = new Room("Left Office In");
            rooms[14] = new Room("Right Office In");
        }

        public static bool operator ==(Room r1, Room r2) => r1.name == r2.name;
        public static bool operator !=(Room r1, Room r2) => r1.name != r2.name;

        public override string ToString() => name;
        public override bool Equals(object obj) => obj is Room r && r.name == name;
        public override int GetHashCode() => name.GetHashCode();
    }
}
