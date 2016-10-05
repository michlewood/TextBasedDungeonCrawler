using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    class Room
    {
        private string[] listOfRoomDescriptions; //en sträng som inehåller en beskrivning på rummet
        public string[] ListOfRoomDescriptions
        {
            get
            {
                return listOfRoomDescriptions;
            }

            set
            {
                listOfRoomDescriptions = value;
            }
        }

        private string roomDescription; //en sträng som inehåller en beskrivning på rummet
        public string RoomDescription
        {
            get
            {
                return roomDescription;
            }

            private set
            {
                roomDescription = value;
            }
        }

        private Enemy enemy; //fiende som finns i rummet
        public Enemy Enemy
        {
            get
            {
                return enemy;
            }

            private set
            {
                enemy = value;
            }
        }

        private Enemy deadEnemy; //om det fanns en fiende förut som nu är död
        public Enemy DeadEnemy
        {
            get
            {
                return deadEnemy;
            }

            private set
            {
                deadEnemy = value;
            }
        }

        private Item item; // om det är någon item i rummet
        public Item Item
        {
            get
            {
                return item;
            }

            private set
            {
                item = value;
            }
        }

        private Connection[] exits = new Connection[4]; // en lista på potentiella utgångra från rummet
        public Connection[] Exits
        {
            get
            {
                List<Connection> temp = new Connection().Doors.FindAll(x => x.RoomOne == this);
                exits[0] = temp.Find(x => x.DirectionFromRoomOne == 0);
                exits[1] = temp.Find(x => x.DirectionFromRoomOne == 1);
                exits[2] = temp.Find(x => x.DirectionFromRoomOne == 2);
                exits[3] = temp.Find(x => x.DirectionFromRoomOne == 3);
                return exits;
            }
        }

        private bool respawn; // om fienden i rummet ska kunna respawna
        internal bool Respawn
        {
            get
            {
                return respawn;
            }

            private set
            {
                respawn = value;
            }
        }

        private NPC npc; //om det finn en npc i rummet
        internal NPC NPC
        {
            get
            {
                return npc;
            }

            set
            {
                npc = value;
            }
        }

        private readonly int positionInMap;
        public int PositionInMap
        {
            get
            {
                return positionInMap;
            }
        }

        public string TypeOfRoom { get; private set; }

        /*public Room(NPC npc, Item item, Enemy enemy, bool respawn, int positionInMap, string roomDescription, string newDescriptionIfEnemyIsRemovedFromRoom,
            string newDescriptionIfItemIsRemovedFromRoom) //konstruktor för Room (ska den inte ha t ex en fiende så skriv null)
        {
            RoomDescription = roomDescription;
            NewDescriptionIfEnemyIsRemovedFromRoom = newDescriptionIfEnemyIsRemovedFromRoom;
            NewDescriptionIfItemIsRemovedFromRoom = newDescriptionIfItemIsRemovedFromRoom;
            this.positionInMap = positionInMap;
            Enemy = enemy;
            Item = item;
            Respawn = respawn;
            NPC = npc;
        }*/

        protected Room(int positionInMap, string roomDescription, string typeOfRoom)
        {
            this.RoomDescription = roomDescription;
            this.positionInMap = positionInMap;
            TypeOfRoom = typeOfRoom;
        }

        public Room(NPC npc, Item item, Enemy enemy, bool respawn, int positionInMap, params string[] roomDescriptions) //konstruktor för Room (ska den inte ha t ex en fiende så skriv null)
        {
            ListOfRoomDescriptions = roomDescriptions;
            RoomDescription = ListOfRoomDescriptions[0];

            this.positionInMap = positionInMap;
            Enemy = enemy;
            Item = item;
            Respawn = respawn;
            NPC = npc;
        }

        internal string GetExitsAsString() //en sträng av alla utgångar 
        {

            string ExitsText = "";

            if (!(Exits[0] == null)) ExitsText += "North ";

            if (!(Exits[1] == null)) ExitsText += "South ";

            if (!(Exits[2] == null)) ExitsText += "East ";

            if (!(Exits[3] == null)) ExitsText += "West ";


            return ExitsText;
        }

        internal void RemoveItem() //tar bort ett item från rummet
        {
            if (!(item == null))
            {
                if (!(ListOfRoomDescriptions[2] == null))
                {
                    RoomDescription = ListOfRoomDescriptions[2];
                    item = null;
                }

            }
        }

        internal void RemoveEnemy() // tar bort fienden från rummet
        {

            DeadEnemy = Enemy;
            Enemy = null;
        }

        internal void UpdateRoomIfEnemyIsRemovedFromRoom() // updaterar rumbeskrivningen 
        {
            if (ListOfRoomDescriptions[1] != null)
            {
                RoomDescription = ListOfRoomDescriptions[1];
            }
        }

        internal void UpdateRoomIfItemIsRemovedFromRoom() // updaterar rumbeskrivningen 
        {
            if (ListOfRoomDescriptions[2] != null)
            {
                RoomDescription = ListOfRoomDescriptions[2];
            }
        }

        internal void RespawnEnemy() //återupplivar fienden i rummet 
        {
            Enemy = DeadEnemy;
            Enemy.Reset();
            DeadEnemy = null;
            RoomDescription = ListOfRoomDescriptions[0];
        }
    }

    class StairRoom : Room
    {
        public bool StairsGoUp { get; private set; }
        public StairRoom(int positionInMap, string roomDescription, bool stairsGoUp) : base(positionInMap, roomDescription, "Stairroom")
        {
            StairsGoUp = stairsGoUp;
        }

        public void UseStairs()
        {
            int floorNumber = Program.roomGenerator.FloorNumber;
            if(StairsGoUp) Program.roomGenerator = new RoomGenerator(floorNumber+1);
            else Program.roomGenerator = new RoomGenerator(floorNumber-1);
        }
    }

}
