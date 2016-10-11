﻿using System;

namespace TextDungeon
{
    public abstract class Enemy : Creature  //en abstrakt klass som alla fiender ärver ifrån. ärver i sin tur från creature
    {
        public int Money { get; protected set; }

        protected int minMoney; // en int för det minsta mängd pengar spelaren kan få när hen dödar en fiende
        protected int maxMoney; // en int för det mesta mängd pengar spelaren kan få  när hen dödar en fiende

        public string AttackName { get; protected set; }

        public bool WinIfKilled { get; protected set; }

        public bool IsAgressive { get; protected set; }

        public Enemy(string name, int maxHp, string attackName, int exp, int minMoney, int maxMoney, bool winIfKilled) : base (name, maxHp) //konstruktor för råtta
        {
            AttackName = attackName;
            Hp = MaxHp;
            Exp = exp;
            Money = GetRNG(minMoney, maxMoney);
            WinIfKilled = winIfKilled;
        }

        internal void Reset() //återställer en fiendes värden om den ska återupplivas 
        {
            Hp = MaxHp;
            Money = GetRNG(minMoney, maxMoney);
        }
    }

    public class Rat : Enemy //fiende: råtta 
    {
        public Rat(bool winIfKilled) : base("Rat", 2, "Nibble", 1, 1, 3, winIfKilled) //konstruktor för råtta
        {

        }

        public Rat() : base("Rat", 2, "Nibble", 2, 2, 3, false) //konstruktor för råtta
        {

        }


        public override int Attack() // råttans attack 
        {
            return GetRNG(0, 3);
        }

    }

    public class Dog : Enemy // fiende: hund
    {
        public Dog() : base ("Dog", 5, "Bite", 3, 4, 6, false) //konstruktor för hund 
        {

        }

        public override int Attack() // hundens attack 
        {
            return GetRNG(1, 4);
        }
    }

    public class Ghidorah : Enemy
    {
        public Ghidorah(bool winIfKilled) : base("Ghidorah", 100, "Triple bite", 50, 50, 151, winIfKilled) //konstruktor för Ghidorah 
        {
            IsAgressive = true;
        }

        public override int Attack()
        {
            return GetRNG(50, 60);
        }
    }
}