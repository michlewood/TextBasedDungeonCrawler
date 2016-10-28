using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    class Room
    {
        #region Class variables and properties
        public string[] ListOfRoomDescriptions { get; set; } //en sträng som inehåller en beskrivning på rummet

        public string RoomDescription { get; private set; } //en sträng som inehåller en beskrivning på rummet

        public Enemy Enemy { get; private set; } //fiende som finns i rummet
        public Enemy DeadEnemy { get; private set; } //om det fanns en fiende förut som nu är död
        internal bool Respawn { get; private set; } // om fienden i rummet ska kunna respawna

        public Item Item { get; private set; } // om det är någon item i rummet

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

        internal NPC NPC { get; private set; } //om det finn en npc i rummet

        private readonly int positionInMap;
        public int PositionInMap
        {
            get
            {
                return positionInMap;
            }
        }

        public bool IsWinOnEntry { get; private set; }

        public string TypeOfRoom { get; private set; }
        #endregion

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

        protected Room(int positionInMap, bool isWinOnEntry, string roomDescription, string typeOfRoom)
        {
            this.RoomDescription = roomDescription;
            this.positionInMap = positionInMap;
            TypeOfRoom = typeOfRoom;
            IsWinOnEntry = isWinOnEntry;
        }

        public Room(NPC npc, Item item, Enemy enemy, bool respawn, int positionInMap, bool isWinOnEntry, params string[] roomDescriptions) //konstruktor för Room (ska den inte ha t ex en fiende så skriv null)
        {
            ListOfRoomDescriptions = roomDescriptions;
            RoomDescription = ListOfRoomDescriptions[0];

            this.positionInMap = positionInMap;
            Enemy = enemy;
            Item = item;
            Respawn = respawn;
            NPC = npc;
            IsWinOnEntry = isWinOnEntry;
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
            if (!(Item == null))
            {
                if (!(ListOfRoomDescriptions[2] == null))
                {
                    RoomDescription = ListOfRoomDescriptions[2];
                    Item = null;
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
        public StairRoom(int positionInMap, bool isWinOnEntry, string roomDescription, bool stairsGoUp) : base(positionInMap, isWinOnEntry, roomDescription, "Stairroom")
        {
            StairsGoUp = stairsGoUp;
        }

        public void UseStairs()
        {
            int floorNumber = Runtime.roomGenerator.FloorNumber;
            if(StairsGoUp) Runtime.roomGenerator = new RoomGenerator(floorNumber+1);
            else Runtime.roomGenerator = new RoomGenerator(floorNumber-1);
        }
    }
}
