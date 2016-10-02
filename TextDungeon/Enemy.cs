using System;

namespace TextDungeon
{
    public abstract class Enemy : Creature  //en abstrakt klass som alla fiender ärver ifrån. ärver i sin tur från creature
    {
        public int Money { get; protected set; }

        protected int minMoney; // en int för det minsta mängd pengar spelaren kan få när hen dödar en fiende
        protected int maxMoney; // en int för det mesta mängd pengar spelaren kan få  när hen dödar en fiende

        public string AttackName { get; protected set; }

        public bool WinIfKIlled { get; protected set; }

        public bool IsAgressive { get; protected set; }

        public Enemy() // konstruktorn 
        {

        }

        internal void Reset() //återställer en fiendes värden om den ska återupplivas 
        {
            Hp = MaxHp;
            Money = GetRNG(minMoney, maxMoney);
        }
    }

    public class Rat : Enemy //fiende: råtta 
    {
        public Rat() //konstruktor för råtta
        {
            Name = "Rat";
            AttackName = "Nibble";
            MaxHp = 2;
            Hp = MaxHp;
            Exp = 1;
            minMoney = 1;
            maxMoney = 3;
            Money = GetRNG(minMoney, maxMoney);
        }

        public override int Attack() // råttans attack 
        {
            return GetRNG(0, 3);
        }

    }

    public class Dog : Enemy // fiende: hund
    {
        public Dog() //konstruktor för hund 
        {
            Name = "Dog";
            AttackName = "Bite";
            MaxHp = 5;
            Hp = MaxHp;
            Exp = 3;
            minMoney = 3;
            maxMoney = 6;
            Money = GetRNG(minMoney, maxMoney);
        }

        public override int Attack() // hundens attack 
        {
            return GetRNG(1, 4);
        }
    }

    public class Ghidorah : Enemy
    {
        public Ghidorah() //konstruktor för Ghidorah 
        {
            Name = "Ghidorah";
            AttackName = "Triple bite";
            MaxHp = 100;
            Hp = MaxHp;
            Exp = 100;
            minMoney = 200;
            maxMoney = 501;
            Money = GetRNG(minMoney, maxMoney);
            IsAgressive = true;
            WinIfKIlled = true;

        }

        public override int Attack()
        {
            return GetRNG(50, 60);
        }
    }
}