using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextDungeon.DataStores;
using TextDungeon.Models.Creatures;
using TextDungeon.Models.Items;
using TextDungeon.Models.Items.Equipment;
using TextDungeon.Models.Quests;
using TextDungeon.Models.Rooms;

namespace TextDungeon
{
    class Runtime
    {
        static int currentFloor = 1;
        public int CurrentFloor
        {
            get { return currentFloor; }
            set { currentFloor = value; }
        }

        static bool win = false;
        bool newLocation = true;
        Room currentRoom; //sparar det rum spelaren är i
        public static RoomGenerator roomGenerator = new RoomGenerator(currentFloor); //skapar en RoomGenerator som håller en lista av alla rum som finns
        PlayerCharacter playerCharacter = new PlayerCharacter("Michael"); //Karaktären som spelaren spelar som
        public void Start() //Konstruktorn startar programmet
        {
            Console.Title = "TextBasedDungeonCrawler";
            currentRoom = roomGenerator.GetRoom(1);
            HelpScreen();
            GameLoop();

        }

        private void GameLoop() //loopen som spelet alltid går igenom 
        {
            bool alive = true;

            while (alive && !win)
            {
                GameGUI();
                Printer.PrintLine("What would you like to do?");
                string action = Printer.Reader().ToLower();

                checkAction(action);
                if (playerCharacter.Hp < 1)
                {
                    alive = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Printer.PrintLine("You Died");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.ReadKey(true);
                }

                if (!win) win = currentRoom.IsWinOnEntry;
            }
            if (win)
            {
                Win win = new Win();
                Thread thread = new Thread(win.WinScreen);
                thread.Start();
                while (true)
                {

                    Console.ReadKey(true);

                    win.Stop();
                    thread.Join();
                    return;
                }
            }
        }

        private void GameGUI() //skriver spelarens stats och fiender om de finns 

        {

            Console.Clear();
            Map();
            RoomDescription();
            CreaturesInRoomBar();
            StatBar();


        }

        private void Map()
        {
            int highestPositionInMap = 0;
            foreach (Room room in roomGenerator.RoomList)
            {
                if (room.PositionInMap > highestPositionInMap) highestPositionInMap = room.PositionInMap;
            }
            string[] mapTiles = new string[highestPositionInMap + 1];


            for (int i = 0; i < mapTiles.Length; i++)
            {
                if (roomGenerator.RoomList.Find(x => x.PositionInMap == i) == null) mapTiles[i] = "";
                else if (i == currentRoom.PositionInMap)
                {
                    mapTiles[i] = "@";
                }

                else mapTiles[i] = "#";
            }


            for (int i = 0; i < mapTiles.Length; i++)
            {
                if ((i) % 3 == 0)
                {
                    if (mapTiles[i] == "") Console.Write("   ");
                    else Console.Write("[{0}]", mapTiles[i]);
                }
            }
            Console.WriteLine();
            for (int i = 0; i < mapTiles.Length; i++)
            {
                if ((i) % 3 == 1)
                {
                    if (mapTiles[i] == "") Console.Write("   ");
                    else Console.Write("[{0}]", mapTiles[i]);
                }
            }
            Console.WriteLine(); for (int i = 0; i < mapTiles.Length; i++)
            {
                if ((i) % 3 == 2)
                {
                    if (mapTiles[i] == "") Console.Write("   ");
                    else Console.Write("[{0}]", mapTiles[i]);
                }
            }
            Console.WriteLine();
        }

        private void CreaturesInRoomBar()
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

        private void StatBar() //skriver status bar med info om spelaren
        {
            Console.ForegroundColor = ConsoleColor.White;
            Printer.Print(".:| {5}s hp: {0} | Armor: {4} | Stamina: {1} | Damage {6}-{7} | gp: {2} | Exp to level: {3} | Exits: ", playerCharacter.Hp, playerCharacter.Stamina, PlayerCharacter.AmountOfMoney, (playerCharacter.ExpRequieredToLevelUp - playerCharacter.Exp), playerCharacter.ArmorRating, playerCharacter.Name, playerCharacter.MinDamage, playerCharacter.MaxDamage);

            Console.ForegroundColor = ConsoleColor.Green;
            Printer.Print(currentRoom.GetExitsAsString());

            Console.ForegroundColor = ConsoleColor.White;
            Printer.PrintLine("|:.");

            Console.ForegroundColor = ConsoleColor.Gray;

        }

        private void RoomDescription() //beskriver rummet 
        {
            if (newLocation == true)
            {
                newLocation = false;
                Printer.ClearRoomHistory();
            }
            Printer.PrintLine(currentRoom.RoomDescription);

        }

        private void checkAction(string action) //tar emot ett kommando från spelaren och jämför det med de möjliga tillåtna kommandon 
        {
            Printer.PrintLine("");

            if (action.Equals("greed is good"))
            {
                PlayerCharacter.AddToInventory(new Potion());
                PlayerCharacter.AddToInventory(new Potion());
                PlayerCharacter.AddToInventory(new Key());

                PlayerCharacter.AddToInventory(new Dagger());
                PlayerCharacter.AddToInventory(new Shirt());
                Console.WriteLine("The Gods have answered your prayer!");
                Console.ReadKey(true);
                return;
            }

            if (action.Equals("use stairs") || action.Equals("us"))
            {
                if (currentRoom.TypeOfRoom == "Stairroom")
                {
                    StairRoom tempRoom = (StairRoom)currentRoom;
                    tempRoom.UseStairs();

                    currentRoom = roomGenerator.RoomList.Find(x => x.PositionInMap == currentRoom.PositionInMap);
                    return;
                }
            }

            if (CharacterScreen(action)) return;


            if (action.Equals("help"))
            {
                HelpScreen();
                return;
            }

            if (action.Equals("history"))
            {
                Console.WriteLine(Printer.History);
                Console.ReadKey(true);
                return;
            }

            if (action.Equals("room history") || action.Equals("rh"))
            {
                Console.Clear();
                Console.WriteLine(Printer.RoomHistory);
                Console.ReadKey(true);
                return;
            }

            if (Move(action))
            {
                return;
            }

            if (Attack(action))
            {
                Printer.ReadKey(true);
                return;
            }

            if (StrongAttack(action))
            {
                Printer.ReadKey(true);
                return;
            }

            if (UsePotion(action))
            {
                EnemyAttack();
                Printer.ReadKey(true);
                return;
            }

            if (action.Equals("l") || action.Equals("look"))
            {
                if (!(currentRoom.Item == null)) Printer.PrintLine("There is a {0}", currentRoom.Item.Name);

                else Printer.PrintLine("There is nothing here!");
                EnemyAttack();
                Printer.ReadKey(true);
                return;
            }

            if (TakeItem(action))
            {
                EnemyAttack();
                Printer.ReadKey(true);

                return;
            }
            if (Talk(action))
                if (action.Equals("talk"))
                {

                    Printer.ReadKey(true);
                    return;
                }

            if (action.Equals("quest") || action.Equals("q"))
            {

                if (playerCharacter.CurrentQuest != null)
                {
                    Printer.PrintLine(playerCharacter.CurrentQuest.Name);
                    if (playerCharacter.CurrentQuest.GetType() == new KillQuest().GetType())
                    {
                        KillQuest tempQuest = (KillQuest)playerCharacter.CurrentQuest;
                        Printer.PrintLine("You have killed {0}/{1} {2}", tempQuest.CurrentAmountOfEnemiesKilled, tempQuest.TotalAmountOfEnemies, tempQuest.EnemyToKill.Name);
                    }
                }
                else Printer.PrintLine("No current quest");
                Printer.ReadKey(true);
                return;
            }

            Printer.PrintLine("invalid action");
            Printer.ReadKey(true);
            EnemyAttack();

        }

        #region Screens

        private bool CharacterScreen(string action)
        {
            if (action.Equals("Character") || action.Equals("c") || action.Equals("inventory") || action.Equals("i"))
            {
                string command;
                do
                {
                    Console.Clear();
                    Console.WriteLine(playerCharacter.Name);
                    CharacterStatScreen();
                    EquipmentScreen();
                    CharacterInfoScreen();
                    List<Item> uniqueItems = InventoryScreen();
                    Console.WriteLine("What Would you like to do? (q to exit)");
                    command = Console.ReadLine().ToLower();
                    if (UsePotion(command)) Console.ReadKey(true);
                    else if (Equipt(command, uniqueItems)) Console.ReadKey(true);
                } while (command != "q");
                Console.Clear();
                Console.WriteLine(Printer.RoomHistory);
                return true;
            }
            return false;
        }

        private void CharacterInfoScreen()
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

        private void CharacterStatScreen()
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

        private void EquipmentScreen()
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

        private List<Item> InventoryScreen()
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

        private bool Equipt(string command, List<Item> itemList)
        {
            List<Equipment> equiptableList = new List<Equipment>();
            foreach (Item item in itemList)
            {
                if (item.Equiptable == true)
                {
                    equiptableList.Add((Equipment)item);
                }
            }
            string firstHalvOfCommand = "";
            string secondHalvOfCommand = "";
            if (command.Length > 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    firstHalvOfCommand += "" + command[i];
                }
            }

            if (command.Length > 6 && command[6] != ' ')
            {
                Console.WriteLine("Invalid Input");
                return true;
            }

            for (int i = 7; i < command.Length; i++)
            {

                secondHalvOfCommand += "" + command[i];
            }


            if (firstHalvOfCommand == "equipt")
            {
                Item itemToEquipt = itemList.Find(x => x.Name.ToLower() == secondHalvOfCommand.ToLower());
                if (itemToEquipt != null && itemToEquipt.Equiptable)
                {
                    if (itemToEquipt.IsWeapon)
                    {
                        playerCharacter.EquiptWeapon((Weapon)itemToEquipt);
                        Console.WriteLine("equipted weapon: {0}", playerCharacter.EquiptedWeapon.Name);

                    }
                    else if (itemToEquipt.IsArmor)
                    {
                        playerCharacter.EquiptArmor((Armor)itemToEquipt);
                        Console.WriteLine("equipted armor: {0}", playerCharacter.EquiptedArmor.Name);
                    }
                }

                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("No item to equipt!");
                }

                return true;
            }

            else return false;
        }

        private void HelpScreen()
        {
            Console.Clear();
            Console.WriteLine("Commands in game:\n\"n\" to go north \n\"s\" to go south \n\"e\" to go east \n\"w\" to go west\n\n" +
                "\"l\" to look around the current room \n\"t\" to take an item in the current room\n\"us\" to go up or down stairs\n\n" +
                "\"help\" to get the helpscreen \n\"history\" to see everything that has happend \n\"rh\" to see what has happend in this room\n" +
                "\"q\" to see current quest \n\n\"talk\" to talk to Npcs\n" +
                "\"c\" to open the character screen, \n\"p\" to use potion\n\"Equipt name\" to equipt something with that name\n\n\"a\" to attack \n\"sa\" to do a strong attack");
            Console.ReadKey(true);
        }

        #endregion

        #region Actions (alla tillåtna kommandos)

        private bool Move(string action) //hanterar om spelaren vill gå till ett nytt rum 
        {

            if ((action.Equals("north") || action.Equals("n")) && !(currentRoom.Exits[0] == null))
            {
                newLocation = true;

                if (!(Locked(currentRoom.Exits[0])))
                {
                    currentRoom = ChangeRoom(0);
                    playerCharacter.Recovery();

                    Console.WriteLine("You head north");

                    Printer.ReadKey(true);

                    EnemySneakAttack();
                }

                return true;
            }

            else if ((action.Equals("south") || action.Equals("s")) && !(currentRoom.Exits[1] == null))
            {
                newLocation = true;

                if (!(Locked(currentRoom.Exits[1])))
                {
                    currentRoom = ChangeRoom(1);
                    playerCharacter.Recovery();

                    Console.WriteLine("You head south");
                    Printer.ReadKey(true);

                    EnemySneakAttack();
                }

                return true;
            }

            else if ((action.Equals("east") || action.Equals("e")) && !(currentRoom.Exits[2] == null))
            {
                newLocation = true;

                if (!(Locked(currentRoom.Exits[2])))
                {
                    currentRoom = ChangeRoom(2);
                    playerCharacter.Recovery();

                    Console.WriteLine("You head east");
                    Printer.ReadKey(true);

                    EnemySneakAttack();
                }

                return true;
            }

            else if ((action.Equals("west") || action.Equals("w")) && !(currentRoom.Exits[3] == null))
            {
                newLocation = true;

                if (!(Locked(currentRoom.Exits[3])))
                {
                    currentRoom = ChangeRoom(3);
                    playerCharacter.Recovery();

                    Console.WriteLine("You head west");
                    Printer.ReadKey(true);

                    EnemySneakAttack();
                }

                return true;
            }

            else if (action.Equals("n") || action.Equals("north") ||
                action.Equals("s") || action.Equals("south") ||
                action.Equals("e") || action.Equals("east") ||
                action.Equals("w") || action.Equals("west"))
            {
                Printer.PrintLine("You walk into the wall!");
                EnemyAttack();
                Printer.ReadKey(true);
                return true;
            }

            return false;
        }

        private void EnemySneakAttack()
        {

            if (currentRoom.Enemy != null && currentRoom.Enemy.IsAgressive)
            {
                GameGUI();
                Printer.PrintLine("");
                EnemyAttack();
                Printer.ReadKey(true);
            }
        }

        private Room ChangeRoom(int direction) //byter rum och återupplivare fiende i det rummet om det är relevant
        {
            if (currentRoom.Exits[direction].RoomTwo.Respawn && currentRoom.Exits[direction].RoomTwo.DeadEnemy != null)
            {
                currentRoom.Exits[direction].RoomTwo.RespawnEnemy();
            }
            return currentRoom.Exits[direction].RoomTwo;
        }

        private bool Attack(string action) //gör vanlig attack
        {
            if ((action.Equals("a") || action.Equals("attack") ||
                action.Equals("na") || action.Equals("normal attack"))
                && !(currentRoom.Enemy == null))
            {
                int damage = playerCharacter.Attack();
                damage = currentRoom.Enemy.TakeDamage(damage);

                Console.ForegroundColor = ConsoleColor.Green;
                Printer.PrintLine("You did {0} damage!", damage);
                Console.ForegroundColor = ConsoleColor.Gray;


                EnemyDeath();

                EnemyAttack();

                return true;
            }
            return false;

        }

        private bool StrongAttack(string action) //gör en starkare attack 
        {
            if ((action.Equals("sa") || action.Equals("strong attack")) && !(currentRoom.Enemy == null))

            {
                int damage = playerCharacter.StrongAttack();
                damage = currentRoom.Enemy.TakeDamage(damage);

                Console.ForegroundColor = ConsoleColor.Green;
                Printer.PrintLine("You did {0} damage!", damage);
                Console.ForegroundColor = ConsoleColor.Gray;

                EnemyDeath();

                EnemyAttack();

                return true;
            }

            return false;
        }

        private bool TakeItem(string action) //tar item om det finns i rummet 
        {
            if (action.Equals("t") || action.Equals("take"))
            {
                if (!(currentRoom.Enemy == null))
                {
                    Printer.PrintLine("You can not take items while there is an enemy in the room.");
                }
                else if (!(currentRoom.Item == null))
                {
                    Printer.PrintLine("You took the {0}!", currentRoom.Item.Name);
                    PlayerCharacter.AddToInventory(currentRoom.Item);
                    currentRoom.RemoveItem();
                    currentRoom.UpdateRoomIfItemIsRemovedFromRoom();
                }
                else Printer.PrintLine("There is nothing here!");
                return true;
            }
            return false;
        }

        private bool UsePotion(string action) //använder en potion från spelarens inventory om det finns
        {
            if (action.Equals("p") || action.Equals("potion"))
            {
                int heal = playerCharacter.UsePotion();
                if (heal == 0) Printer.PrintLine("You are out of potions");
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Printer.PrintLine("You heal for {0} hp", heal);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                return true;
            }
            return false;
        }

        private bool Talk(string action)
        {
            if (action.Equals("talk"))
            {

                if (currentRoom.NPC != null)
                {
                    if (currentRoom.NPC.Name.Equals("Shopkeep"))
                    {
                        currentRoom.NPC.Interact();
                    }

                    if (currentRoom.NPC.Name.Equals("Quest Giver") && playerCharacter.CurrentQuest == null)
                    {
                        QuestGiver tempGiver;
                        if (currentRoom.NPC.Interact())
                        {
                            tempGiver = (QuestGiver)currentRoom.NPC;
                            playerCharacter.CurrentQuest = tempGiver.GiveQuest();
                        }
                    }

                    else if (currentRoom.NPC.Name.Equals("Quest Giver") && playerCharacter.CurrentQuest != null)
                    {
                        if (playerCharacter.CurrentQuest.CheckCompleted())
                        {
                            currentRoom.NPC.Interact();

                            playerCharacter.CurrentQuest = null;
                        }

                        else
                        {
                            currentRoom.NPC.Interact();
                        }


                    }

                }

                else Printer.PrintLine("There is no one to talk to!");

                return true;
            }
            return false;
        }

        #endregion  

        private bool Locked(Connection connection) // kollar om en dörr är låst och låter spelaren låsa up den om de har nyckel
        {

            if (connection.NeedKey)
            {
                if (!(currentRoom.Enemy == null))
                {
                    newLocation = false;
                    Printer.PrintLine("You cannot unlock a door while there is an enemy in the room!");
                    return true;
                }

                if (!(PlayerCharacter.Inventory.ItemList.Find(x => x.Name == "Key") == null))
                {
                    newLocation = false;

                    do
                    {
                        Printer.PrintLine("Do you want to unlock the door? (y/n)");
                        string useKey = Printer.Reader().ToLower();

                        if (useKey.Equals("yes") || (useKey.Equals("y")))
                        {

                            Printer.PrintLine("You unlocked the door!");
                            connection.Unlock();
                            PlayerCharacter.Inventory.RemoveItem(PlayerCharacter.Inventory.ItemList.Find(x => x.Name == "Key"));
                            Printer.ReadKey(true);
                            return true;
                        }
                        else if (useKey.Equals("no") || (useKey.Equals("n")))
                        {
                            return true;
                        }

                    } while (true);

                }
                else
                {
                    newLocation = false;
                    Printer.PrintLine("The door is locked!");
                    Printer.ReadKey(true);
                    return true;
                }
            }

            return false;
        }

        private void EnemyAttack() // sköter fiendens attacker
        {
            if (!(currentRoom.Enemy == null))
            {
                int damage = currentRoom.Enemy.Attack();
                damage = playerCharacter.TakeDamage(damage);
                Console.ForegroundColor = ConsoleColor.Red;
                Printer.PrintLine("the {1} {2}s you for {0} damage!", damage, currentRoom.Enemy.Name, currentRoom.Enemy.AttackName.ToLower());
                Console.ForegroundColor = ConsoleColor.Gray;

            }
        }

        private void EnemyDeath() //sköter hur en fiende dör och ger spelaren exp, etc 
        {
            if (currentRoom.Enemy.Hp <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Printer.PrintLine("You killed the {0}!", currentRoom.Enemy.Name);
                Console.ForegroundColor = ConsoleColor.Gray;

                if (!(currentRoom.Enemy.Money == 0))
                {
                    Printer.PrintLine("You Got {0} gc!", currentRoom.Enemy.Money);
                    PlayerCharacter.AddToMoney(currentRoom.Enemy.Money);
                }

                Printer.PrintLine("You Got {0} exp!", currentRoom.Enemy.Exp);
                playerCharacter.AddExp(currentRoom.Enemy.Exp);

                CheckQuest();
                win = currentRoom.Enemy.WinIfKilled;
                currentRoom.RemoveEnemy();
                currentRoom.UpdateRoomIfEnemyIsRemovedFromRoom();

                return;
            }
        }

        private void CheckQuest() //kollar om en fiende i rummet motsvara det som ska dödas för ett quest 
        {
            if (playerCharacter.CurrentQuest != null)
            {
                if (playerCharacter.CurrentQuest.GetType() == new KillQuest().GetType())
                {
                    KillQuest tempQuest = (KillQuest)playerCharacter.CurrentQuest;
                    if (currentRoom.Enemy.Name == tempQuest.EnemyToKill.Name)
                    {
                        tempQuest.UpdateQuest();
                    }
                }
            }
        }
    }
}
