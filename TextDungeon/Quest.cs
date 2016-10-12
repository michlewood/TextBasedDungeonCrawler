using System;

namespace TextDungeon
{
    abstract public class Quest
    {
        public string Name { get; private set; } //namnet på questen
        public int RewardMoney { get; private set; } // pengar man får för att klara questet
        public Item RewardItem { get; private set; } // item som man får för att klara questet

        protected Quest(string name, int rewardMoney, Item rewardItem) //konstruktor för quest (om man t ex inte vill att man ska få ett item skriv null) 
        {
            Name = name;
            RewardMoney = rewardMoney;
            RewardItem = rewardItem;
        }

        protected Quest() { }

        abstract public int UpdateQuest(); // uppdaterar questet 

        abstract public void ResetQuest();

        abstract public bool CheckCompleted(); // kollar om målet i questen är avklarat 
    }

    public class KillQuest : Quest
    {
        public Enemy EnemyToKill { get; private set; }
        public int TotalAmountOfEnemies { get; private set; }
        public int CurrentAmountOfEnemiesKilled { get; private set; }

        public KillQuest() { }

        public KillQuest(string name, Enemy enemyToKill, int totalAmountOfEnemies, int rewardMoney, Item rewardItem) : base(name, rewardMoney, rewardItem)
        {
            EnemyToKill = enemyToKill;
            TotalAmountOfEnemies = totalAmountOfEnemies;
        }

        public override int UpdateQuest() // uppdaterar questet 
        {
            if (TotalAmountOfEnemies != CurrentAmountOfEnemiesKilled)
                CurrentAmountOfEnemiesKilled++;

            return CurrentAmountOfEnemiesKilled;
        }

        public override void ResetQuest()
        {
            CurrentAmountOfEnemiesKilled = 0;
        }

        public override bool CheckCompleted()
        {
            if (CurrentAmountOfEnemiesKilled == TotalAmountOfEnemies) return true;
            else return false;
        }
    }
}