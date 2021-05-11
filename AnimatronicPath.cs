using System;
using System.Collections.Generic;
using System.Text;

namespace FNAF1_Recreation
{
    class AnimatronicPath
    {
        public Dictionary<Room, List<Room>> connections;
        public List<Room> rooms;
        private readonly Rand rand;
        public int roomIndex;

        public AnimatronicPath()
        {
            rooms = new List<Room>();
            connections = new Dictionary<Room, List<Room>>();
            roomIndex = 0;
        }

        public AnimatronicPath(Rand r)
        {
            rooms = new List<Room>();
            connections = new Dictionary<Room, List<Room>>();
            rand = r;
            roomIndex = 0;
        }

        public void AddRoom(Room r)
        {
            rooms.Add(r);
            Debug.WriteLine($"Added room {r} to path");
        }

        public void AddConnection(Room start, Room con)
        {
            List<Room> connected = connections[start];
            if (!connected.Contains(con)) connected.Add(con);
            Debug.WriteLine($"Connected room {con} to {start}");
        }

        public Room NextRoom() => rooms[roomIndex++];

        public Room Random() => rooms[rand.RandInt(0, rooms.Count - 1)];

        public Room Random(Room start) => connections[start][rand.RandInt(0, connections[start].Count - 1)];
    }
}
