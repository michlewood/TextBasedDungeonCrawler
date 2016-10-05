using System;

namespace TextDungeon
{
    internal abstract class Item // en abstrakt klass för items som alla olika items typer ärver ifrån 
    {

        public string Name { get; protected set; } //alla items ska ha ett namn

        public int Price { get; protected set; } // alla items har ett pris

        public bool Equiptable { get; protected set; }

        public bool IsWeapon { get; protected set; }

        public bool IsArmor { get; protected set; }

        protected Item(string name, int price)
        {
            Name = name;
            Price = price;
        }

        abstract public string StorDescription();

    }

    internal class Potion : Item //item: potion 
    {
        private readonly int heal = 25; // den mängd som potions ska hela
        public int Heal { get { return heal; } }

        internal Potion() : base ("Potion", 10) // potions konstruktor
        {

        }

        public override string StorDescription()
        {
            return Name + " - Price: " + Price + "gp - Heals for " + heal + " hp ";
        }
    }

    internal class Key : Item //item: key - låser upp dörrar
    {
        internal Key() : base ("Key", 50) // konstruktor för key 
        {

        }

        public override string StorDescription()
        {
            return "Unlock doors";
        }
    }
}