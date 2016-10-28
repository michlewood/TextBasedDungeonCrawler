using System;
using System.Collections.Generic;
using TextDungeon.Models.Rooms;

namespace TextDungeon.DataStores
{
    internal class Connection
    {
        static private List<Connection> doors = new List<Connection>(); // en lista på alla dörras som skapats
        public List<Connection> Doors
        {
            get
            {
                return doors;
            }
        }

        private Room roomOne; //första rummet som är kopplat till dörren
        public Room RoomOne
        {
            get
            {
                return roomOne;
            }

            private set
            {
                roomOne = value;
            }
        }

        private Room roomTwo; // andra rummet som är kopplat till dörren
        public Room RoomTwo
        {
            get
            {
                return roomTwo;
            }

            private set
            {
                roomTwo = value;
            }
        }

        private int directionFromRoomOne; // vilket håll från rum1
        public int DirectionFromRoomOne
        {
            get
            {
                return directionFromRoomOne;
            }

            private set
            {
                directionFromRoomOne = value;
            }
        }

        private bool needKey; // om dörren behöver en nyckel
        public bool NeedKey
        {
            get
            {
                return needKey;
            }

            private set
            {
                this.needKey = value;
            }
        }

        public Connection(Room roomOne, Room roomTwo, bool needKey)
        {
            RoomOne = roomOne;
            RoomTwo = roomTwo;

            int relativePosition = RoomOne.PositionInMap - RoomTwo.PositionInMap;
            int otherDirection;
            if (relativePosition == 1) { DirectionFromRoomOne = 0; otherDirection = 1; }
            else if (relativePosition == -1) { DirectionFromRoomOne = 1; otherDirection = 0; }
            else if (relativePosition == -3) { DirectionFromRoomOne = 2; otherDirection = 3; }
            else if (relativePosition == 3) { DirectionFromRoomOne = 3; otherDirection = 2; }
            else { DirectionFromRoomOne = 4; otherDirection = 4; }

            NeedKey = needKey;

            Connection tempConnection = new Connection();
            tempConnection.RoomOne = roomTwo;
            tempConnection.RoomTwo = roomOne;
            tempConnection.DirectionFromRoomOne = otherDirection;
            NeedKey = needKey;



            Doors.Add(tempConnection);
        }

        internal Connection() { }

        internal void Unlock()
        {
            NeedKey = false;
            Doors.Find(x => x.RoomOne == RoomTwo && x.RoomTwo == RoomOne).NeedKey = false;
        }

        internal class AddDoor
        {

            public AddDoor(Connection newDoor)
            {
                doors.Add(newDoor);

            }
        }

    }
}