using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    public abstract class Creature
    {
        private Random rng = new Random();

        public string Name { get; protected set; }

        public int MaxHp { get; protected set; }

        protected int hp;
        public int Hp
        {
            get { return hp; }
            protected set { hp = value; if (hp > MaxHp) hp = MaxHp; }
        }

        private int exp;
        public int Exp { get; protected set; }

        protected int armorRating = 0;
        public int ArmorRating
        {
            get { return armorRating; }
            protected set { armorRating = value; }
        }

        protected Creature(string name, int maxHp)
        {
            Name = name;
            Hp = MaxHp = maxHp;
        }

        public abstract int Attack();

        public int TakeDamage(int amountOfDamage)
        {
            int damage = amountOfDamage - ArmorRating;
            if (damage > 0)
            {
                Hp -= damage;
                return damage;
            }
            else return 0;
        }

        private void checkHp()
        {

            if (Hp > MaxHp) Hp = MaxHp;
        }

        protected int GetRNG(int min, int max)
        {
            return rng.Next(min, max);
        }

    }
}
