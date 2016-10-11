using System;
using System.Collections.Generic;

namespace TextDungeon
{
    public class PlayerCharacter : Creature
    {
        #region Variables and Properties

        private Inventory inventory = new Inventory();
        internal Inventory Inventory
        {
            get
            {
                return inventory;
            }
        }

        static private int amountOfMoney = 10;
        internal static int AmountOfMoney
        {
            get
            {
                int tempMoney = amountOfMoney;
                return tempMoney;
            }
        }

        private int level = 1; //spelarens level börjar på 1
        public int Level
        {
            get
            {
                return level;
            }

            set
            {
                level = value;
            }
        }

        private static int expRequieredToLevelUp;
        public int ExpRequieredToLevelUp
        {
            get
            {
                expRequieredToLevelUp = 5 + (level * 5);
                return expRequieredToLevelUp;
            }
        }

        private int stamina = 10; // hur mycket stamina spelaren hur just nu
        public int Stamina
        {
            get
            {
                return stamina;
            }

            set
            {
                stamina = value;
                if (stamina > MaxStamina) stamina = MaxStamina;
            }
        }

        private int maxStamina = 10; // hur mycket stamina spelaren har som max
        public int MaxStamina
        {
            get
            {
                return maxStamina;
            }

            set
            {
                maxStamina = value;
            }

        }

        private int minDamage;
        public int MinDamage
        {
            get
            {
                int weaponDamage = 0;
                if (EquiptedWeapon != null)
                {
                    weaponDamage = EquiptedWeapon.DamageDone;
                }
                minDamage = 0 + level + weaponDamage;
                return minDamage;
            }
            private set { minDamage = value; }
        }

        private int maxDamage;
        public int MaxDamage
        {
            get
            {
                int weaponDamage = 0;
                if (EquiptedWeapon != null)
                {
                    weaponDamage = EquiptedWeapon.DamageDone;
                }
                maxDamage = 3 + level + weaponDamage;
                return maxDamage;
            }
            private set { maxDamage = value; }
        }

        #region stats
        private int happiness = 1;
        public int Happiness
        {
            get
            {
                return happiness;
            }

            private set
            {
                happiness = value;
            }
        }

        private int determination = 1;
        public int Determination
        {
            get
            {
                return determination;
            }

            private set
            {
                determination = value;
            }
        }
        #endregion

        Random rng = new Random();

        public Quest CurrentQuest { get; set; } //det quest som spelaren har just nu

        internal Weapon EquiptedWeapon { get; private set; }

        private Armor equiptedArmor = new Shirt();

        internal Armor EquiptedArmor { get; private set; }

        public new int ArmorRating
        {
            get
            {
                if (EquiptedArmor != null)
                {
                    ArmorRating = EquiptedArmor.ArmorRating;
                    return armorRating;
                }

                else
                {
                    ArmorRating = 0;
                    return 0;
                }

            }

            protected set
            {
                armorRating = value;
            }

        }

        #endregion

        public PlayerCharacter(string name) : base(name, 30) //konstruktorn som skapar en ny spelare
        {
            AddToInventory(new Potion());

        }

        internal void AddToInventory(Item item) // metod som lägger till en item i inventory
        {
            inventory.AddItem(item);

        }

        public override int Attack() //metod som returnerar hur mycket skada spelaren ska göra med en simpel attack
        {

            return GetRNG(MinDamage, MaxDamage);

        }

        public int StrongAttack() //metod som returnerar hur mycket skada spelaren ska göra med en stark attack (använder stamina)
        {
            if (stamina >= 3)
            {
                stamina -= 3;
                return GetRNG(MinDamage+1,MaxDamage+5);

            }
            else
            {
                Console.WriteLine("Out of stamina!");
                return 0;
            }
        }

        public int UsePotion() //metod för att använda en potion och hela spelaren 
        {
            Potion tempPotion = (Potion)Inventory.ItemList.Find(x => x.Name == "Potion");

            if (tempPotion == null) return 0;

            HealDamage(tempPotion.Heal);
            Inventory.RemoveItem(tempPotion);
            return tempPotion.Heal;
        }

        private void HealDamage(int heal)
        {
            hp += heal;
            checkHp();
        }

        private void checkHp()
        {
            if (hp > MaxHp) hp = MaxHp;
        }

        internal void EquiptWeapon(Weapon weaponToEquipt)
        {
            if (EquiptedWeapon != null) AddToInventory(EquiptedWeapon);
            Inventory.RemoveItem(weaponToEquipt);
            EquiptedWeapon = weaponToEquipt;
        }

        internal void EquiptArmor(Armor armorToEquipt)
        {
            if (EquiptedArmor != null) AddToInventory(EquiptedArmor);
            Inventory.RemoveItem(armorToEquipt);
            EquiptedArmor = armorToEquipt;
            ArmorRating = armorToEquipt.ArmorRating;
        }

        internal static void AddToMoney(int amountToAdd) //metod för att ge spelaren pengar (t ex om hen besegrar en fiende) 
        {
            amountOfMoney += amountToAdd;
        }

        public void AddExp(int exp) //metod för att ge spelaren exp (t ex om hen besegrar en fiende) 
        {
            Exp += exp;
            LevelUp();

        }

        private void LevelUp() //metod för att se om spelaren har exp för att levla och sköter det (kan hantera mer än en level i taget)
        {
            while (true)
            {
                if (Exp >= ExpRequieredToLevelUp)
                {
                    Exp -= ExpRequieredToLevelUp;
                    Level++;

                    Console.WriteLine("You leveled up! \nyou are level {0}!", level);

                    MaxHp += 5;
                    HealDamage(5);
                    MaxStamina += 2;
                    Stamina = MaxStamina;
                    Console.WriteLine("Hp increased by 5");
                    Console.WriteLine("Stamina increased by 2");
                    int statIncrease = rng.Next(1, 3);
                    Console.WriteLine("Happiness increased by {0}", statIncrease);
                    Happiness += statIncrease;
                    statIncrease = rng.Next(1, 3);
                    Console.WriteLine("Determination increased by {0}", statIncrease);
                    Determination += statIncrease;




                }
                else return;
            }
        }

        internal void Recovery() //Metod som sköter vad spelaren ska få tillbaka när hen går omkirng 
        {
            Hp += MaxHp / 10;
            Stamina += MaxStamina / 5;
        }

        
    }
}