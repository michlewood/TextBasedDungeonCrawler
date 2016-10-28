using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextDungeon.DataStores;
using TextDungeon.Models.Creatures;
using TextDungeon.Models.Items;
using TextDungeon.Models.Rooms;

namespace TextDungeon
{
    class Graphics
    {
        public static void Map(Room currentRoom, RoomGenerator roomGenerator)
        {
            int highestPositionInMap = 0;
            foreach (Room room in roomGenerator.RoomList)
            {
                if (room.PositionInMap > highestPositionInMap) highestPositionInMap = room.PositionInMap;
            }
            string[] mapTiles = new string[highestPositionInMap + 1];


            for (int i = 0; i < mapTiles.Length; i++)
            {
                if (roomGenerator.RoomList.Find(x => x.PositionInMap == i) == null) mapTiles[i] = "   ";
                else if (i == currentRoom.PositionInMap)
                {
                    mapTiles[i] = "[@]";
                }
                else mapTiles[i] = "[#]";
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = i; j < mapTiles.Length; j += 3)
                {
                    Console.Write("{0}", mapTiles[j]);
                }
                Console.WriteLine();
            }
        }

        public static void CreaturesInRoomBar(Room currentRoom)
        {
            if (!(currentRoom.Enemy == null))
            {
                Printer.Print("Enemy: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Printer.Print(currentRoom.Enemy.Name);
                Console.ForegroundColor = ConsoleColor.Gray;
                Printer.Print(" | hp: " + currentRoom.Enemy.Hp);
            }
            else
            {
                Printer.Print("Enemy: ");
                Console.ForegroundColor = ConsoleColor.White;
                Printer.Print("None");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Printer.Print(" | ");

            if (!(currentRoom.NPC == null))
            {
                Printer.Print("NPC: ");
                Console.ForegroundColor = ConsoleColor.White;
                Printer.Print(currentRoom.NPC.Name);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Printer.Print("NPC: ");
                Console.ForegroundColor = ConsoleColor.White;
                Printer.Print("None");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Printer.PrintLine("");
        }

        public static void StatBar(Room currentRoom, PlayerCharacter playerCharacter) //skriver status bar med info om spelaren
        {
            Console.ForegroundColor = ConsoleColor.White;
            Printer.Print(".:| {5}s hp: {0} | Armor: {4} | Stamina: {1} | Damage {6}-{7} | gp: {2} | Exp to level: {3} | Exits: ", playerCharacter.Hp, playerCharacter.Stamina, PlayerCharacter.AmountOfMoney, (playerCharacter.ExpRequieredToLevelUp - playerCharacter.Exp), playerCharacter.ArmorRating, playerCharacter.Name, playerCharacter.MinDamage, playerCharacter.MaxDamage);

            Console.ForegroundColor = ConsoleColor.Green;
            Printer.Print(currentRoom.GetExitsAsString());

            Console.ForegroundColor = ConsoleColor.White;
            Printer.PrintLine("|:.");

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void RoomDescription(Room currentRoom) //beskriver rummet 
        {
            if (Runtime.newLocation == true)
            {
                Runtime.newLocation = false;
                Printer.ClearRoomHistory();
            }
            Printer.PrintLine(currentRoom.RoomDescription);
        }

        public static void CharacterInfoScreen(PlayerCharacter playerCharacter)
        {
            int row = 1;
            int column = 0;

            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Hp : {0}/{1}", playerCharacter.Hp, playerCharacter.MaxHp);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Stamina : {0}/{1}", playerCharacter.Stamina, playerCharacter.MaxStamina);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Armor Rating : {0}", playerCharacter.ArmorRating);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Damage : {0}-{1}", playerCharacter.MinDamage, playerCharacter.MaxDamage);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Gp : {0}", PlayerCharacter.AmountOfMoney);

            Console.SetCursorPosition(0, row + 1);
        }

        public static void CharacterStatScreen(PlayerCharacter playerCharacter)
        {
            int row = 1;
            int column = 20;
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Level : {0}", playerCharacter.Level);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Exp : {0}/{1}", playerCharacter.Exp, playerCharacter.ExpRequieredToLevelUp);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Stats:");
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Happiness: {0}", playerCharacter.Happiness);
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Determination: {0}", playerCharacter.Determination);
        }

        public static void EquipmentScreen(PlayerCharacter playerCharacter)
        {
            int row = 1;
            int column = 40;
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Equipment:");
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Armor: {0}", playerCharacter.EquiptedArmor != null ? playerCharacter.EquiptedArmor.Name : "None");
            Console.SetCursorPosition(column, row++);
            Console.WriteLine("Weapon: {0}", playerCharacter.EquiptedWeapon != null ? playerCharacter.EquiptedWeapon.Name : "None");

        }

        public static List<Item> InventoryScreen()
        {

            List<Item> uniqueItems = new List<Item>();

            if (PlayerCharacter.Inventory.ItemList.Count != 0)
            {

                Console.WriteLine("Your inventory contains: ");

                foreach (Item item in PlayerCharacter.Inventory.ItemList)
                {
                    if ((uniqueItems.Find(x => x.Name == item.Name)) == null)
                        uniqueItems.Add(item);
                }

                foreach (Item item in uniqueItems)
                {
                    List<Item> numberOfItems = PlayerCharacter.Inventory.ItemList.FindAll(x => x.Name == item.Name);
                    Console.WriteLine(item.Name + " " + numberOfItems.Count);
                }
            }

            else Console.WriteLine("Inventory is empty!");

            return uniqueItems;
        }

        public static void HelpScreen()
        {
            Console.Clear();
            Console.WriteLine("Commands in game:\n\"n\" to go north \n\"s\" to go south \n\"e\" to go east \n\"w\" to go west\n\n" +
                "\"l\" to look around the current room \n\"t\" to take an item in the current room\n\"us\" to go up or down stairs\n\n" +
                "\"help\" to get the helpscreen \n\"history\" to see everything that has happend \n\"rh\" to see what has happend in this room\n" +
                "\"q\" to see current quest \n\n\"talk\" to talk to Npcs\n" +
                "\"c\" to open the character screen, \n\"p\" to use potion\n\"Equipt name\" to equipt something with that name\n\n\"a\" to attack \n\"sa\" to do a strong attack");
            Console.ReadKey(true);
        }
    }
}
