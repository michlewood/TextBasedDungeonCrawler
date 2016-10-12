using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    class RoomGenerator
    {
        
        static List<Room> roomList = new List<Room>(); //listan över all rum i spelet
        internal readonly int FloorNumber;

        internal List<Room> RoomList
        {
            get
            {
                return roomList;
            }

            set
            {
                roomList = value;
            }
        }

        internal int Length
        {
            get
            {
                return RoomList.Count;
            }
        }

        public RoomGenerator(int floor) // konstructor för RoomGenerator 
        {
            FloorNumber = floor;

            RoomList.Clear();
            if (floor == 1) { FirstFloorRooms(); FirstFloorDoors(); }
            if (floor == 2) { SecondFloorRooms(); SecondFloorDoors(); }
        }
        
        #region Second Floor
        private void SecondFloorDoors()
        {
            new Connection.AddDoor(new Connection(RoomList.ElementAt(0), RoomList.ElementAt(1), false));
        }

        private void SecondFloorRooms()
        {
            RoomList.Add(new Room(null, null, new Rat(true), false, 7, false, "new room", null, null));
            RoomList.Add(new StairRoom(8, false, "You see stairs going down", false));
        }
        #endregion

        #region FirstFloor
        private void FirstFloorRooms() //metod som skapar alla rum som finns i spelet
        {
            
            Quest quest = new KillQuest("Kill rats!", new Rat(), 2, 10, new Potion());
            QuestGiver quester = new QuestGiver(quest);
            RoomList.Add(new Room(null, null, new Rat(), true, 0, false, "A giant (relatively speaking) rat  is standning infront of you.", "The rat is dead but, judging by the sounds coming from the wall, there will be more.", null));
            RoomList.Add(new Room(null, null, null, false, 1, false, "You are in a small unadorned room with only torchlight to see by.", null, null));
            RoomList.Add(new Room(null, new Key(), new Dog(), false, 2, false, "A wild dog stands in the middle of the room.", "The corpse of the dog lies in a pool of it own blood. you see a glimmer in the corner of the room.", "The corpse of the dog lies in a pool of it own blood."));
            RoomList.Add(new Room(new Shopkeep(), null, null, false, 3, false, "You are in what is unmistakably a shop. You see a woman sitting behind the counter.", null, null));
            RoomList.Add(new Room(quester, new Potion(), null, false, 4, false, "You see a man standing in the middle of the room. \nthere is a potion on a table by the wall.", null, "You see a man standing in the middle of the room."));
            RoomList.Add(new Room(null, null, new Ghidorah(), false, 7, false, "You see a giant, golden-scaled, three-headed dragon infront of you, you realise that you know it from legends. Ghidorah!", "You have slaint the legendary beast! \nYet greater mysteries remain: How did it fit in this room? And how did it get in here? \nYou will probably never find the answers to these questions", null));
            RoomList.Add(new StairRoom(8, false, "You see stairs going up", true));

        }

        private void FirstFloorDoors() // metod som skapar alla dörrar som rummen sitter ihop med som i sin tur leder till ett annat rum 
        {
            new Connection.AddDoor(new Connection(RoomList.ElementAt(1), RoomList.ElementAt(0), false));

            new Connection.AddDoor(new Connection(RoomList.ElementAt(1), RoomList.ElementAt(2), false));

            new Connection.AddDoor(new Connection(RoomList.ElementAt(1), RoomList.ElementAt(4), true));

            new Connection.AddDoor(new Connection(RoomList.ElementAt(3), RoomList.ElementAt(4), false));

            new Connection.AddDoor(new Connection(RoomList.ElementAt(4), RoomList.ElementAt(5), false));

            new Connection.AddDoor(new Connection(RoomList.ElementAt(5), RoomList.ElementAt(6), false));

        }
        #endregion

        public Room GetRoom(int requestedRoom) //hämtar ett rum vid en specifik index 
        {
            return RoomList.ElementAt(requestedRoom);
        }

    }
}
