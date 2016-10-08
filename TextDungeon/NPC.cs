using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    abstract class NPC : Creature //abstrakt klass NPC som alla npc:er ärver ifrån. Ärver själv från Creature
    {

        protected NPC(string name) : base(name, 10)
        {

        }

        internal abstract bool Interact(); // alla npc:er ska kunna tala

        public override int Attack()
        {
            Console.WriteLine("NPCs cannot attack!");
            throw new NotImplementedException();
        }

    }

    class QuestGiver : NPC //klass : QuestGiver som ärver från NPC
    {

        public Quest Quest { get; protected set; } //den quest som questgivern har
        public bool QuestGiven { get; protected set; } // har questgivern redan get sitt quest till spelaren

        public QuestGiver(Quest quest) : base ("Quest Giver")// konstruktorn för questgivern
        {
            Quest = quest;
        }

        internal override bool Interact() // questgivern talar frågar om man vill ha ett quest och tackar när man är klar 
        {

            if (Quest.CheckCompleted())
            {

                Printer.PrintLine("{0}: Thank you very much!", Name);
                Quest.ResetQuest();
                QuestGiven = false;

            }

            else if (QuestGiven)
            {

                Printer.PrintLine("{0}: Please kill the rats!", Name);


            }
            else
            {
                string answer;
                do
                {

                    Printer.PrintLine("{0}: Will you kill some rats for me? (y/n)", Name);
                    answer = Printer.Reader().ToLower();
                    if (answer == "y" || answer == "yes")
                    {
                        Printer.PrintLine("{0}: Thank You!", Name);
                        GiveQuest();

                        return true;
                    }
                    else if (answer == "n" || answer == "no")
                    {
                        Printer.PrintLine("{0}: Oh well", Name);
                        return false;
                    }



                } while (true);
            }

            return false;

        }

        internal Quest GiveQuest() // questgivern ger sitt quest
        {
            QuestGiven = true;
            return Quest;
        }
    }

    class Shopkeep : NPC
    {
        private Item[] itemList = { new Potion(), new Dagger(), new Shirt() };

        public Shopkeep() : base ("Shopkeep")
        {
            
        }

        internal override bool Interact()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("{0}: Welcome to my shop", Name);
                Console.WriteLine("Your gp: {0}", PlayerCharacter.AmountOfMoney);
                Console.WriteLine("What would you like to buy:");

                foreach (Item item in itemList)
                {

                    Console.WriteLine(item.StorDescription());
                }

                Console.WriteLine();
                Console.WriteLine("Type the name of an item to buy? (q to quit)");
                string answer = Console.ReadLine();

                Item tempItem = null;
                foreach (Item item in itemList)
                {
                    if (item.Name.ToLower() == answer.ToLower())
                    {
                        tempItem = item;
                    }
                }

                if (tempItem != null)
                {
                    if (TryToBuyItem(tempItem))
                    {
                        Console.WriteLine("{0} Thank you. Anything else?", Name);
                    }
                    else Console.WriteLine("You can't afford that");
                    Console.ReadLine();
                }

                else if (answer.Equals("q"))
                {
                    Console.WriteLine("Goodbye!");
                    return true;
                }

                else
                {
                    Console.WriteLine("I don't understand you");
                    Console.ReadLine();
                }
            }
        }

        private bool TryToBuyItem(Item item)
        {
            if (PlayerCharacter.AmountOfMoney >= item.Price)
            {
                new Inventory().AddItem(item);
                PlayerCharacter.AddToMoney(-item.Price);
                return true;
            }

            else return false;

        }
    }

}
