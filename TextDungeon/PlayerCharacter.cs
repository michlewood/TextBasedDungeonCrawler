using System;
using System.Collections.Generic;

namespace TextDungeon
{
    public class PlayerCharacter : Creature
    {
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

        public Quest CurrentQuest { get; set; } //det quest som spelaren har just nu

        internal Weapon EquiptedWeapon { get; private set; }

        internal Armor EquiptedArmor { get; private set; }

        public new int ArmorRating
        {
            get
            {
                if (EquiptedArmor != null)
                {
                    ArmorRating = EquiptedArmor.ArmorRating;
                    return ArmorRating;
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

        public PlayerCharacter(string name) //konstruktorn som skapar en ny spelare
        {
            Name = name;
            MaxHp = 30;
            Hp = MaxHp;
            AddToInventory(new Potion());

        }

        internal void AddToInventory(Item item) // metod som lägger till en item i inventory
        {
            inventory.AddItem(item);

        }

        public override int Attack() //metod som returnerar hur mycket skada spelaren ska göra med en simpel attack
        {
            int weaponDamage = 0;
            if (EquiptedWeapon != null)
            {
                weaponDamage = EquiptedWeapon.DamageDone;
            }
            return GetRNG(0 + level + weaponDamage, 3 + level + weaponDamage);

        }

        public int StrongAttack() //metod som returnerar hur mycket skada spelaren ska göra med en stark attack (använder stamina)
        {

            int weaponDamage = 0;
            if (EquiptedWeapon != null)
            {
                weaponDamage = EquiptedWeapon.DamageDone;
            }

            if (stamina >= 3)
            {
                stamina -= 3;
                return GetRNG(1 + level + weaponDamage, 5 + level + weaponDamage);

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

                    MaxHp += 5;
                    TakeDamage(5);
                    MaxStamina += 2;
                    Stamina = MaxStamina;

                    Console.WriteLine("You leveled up! \nyou are level {0}!", level);

                }
                else return;
            }
        }

        internal void Recovery() //Metod som sköter vad spelaren ska få tillbaka när hen går omkirng 
        {
            Hp += MaxHp / 10;
            Stamina += MaxStamina / 5;
        }

        internal void EquiptArmor(Armor armorToEquipt)
        {
            if (EquiptedArmor != null) AddToInventory(EquiptedArmor);
            Inventory.RemoveItem(armorToEquipt);
            EquiptedArmor = armorToEquipt;
            ArmorRating = armorToEquipt.ArmorRating;
        }
    }
}