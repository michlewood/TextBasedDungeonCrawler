using System;

namespace TextDungeon
{
    abstract public class Quest
    {
        private string name; //namnet på questen
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        } 

        private int rewardMoney; // pengar man får för att klara questet
        public int RewardMoney
        {
            get
            {
                return rewardMoney;
            }
            set
            {
                rewardMoney = value;
            }
        }

        private Item rewardItem; // item som man får för att klara questet
        internal Item RewardItem
        {
            get
            {
                return rewardItem;
            }
            set
            {
                rewardItem = value;
            }

        }

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

        private Enemy enemyToKill; //typen av fiende som ska dödas
        public Enemy EnemyToKill
        {
            get
            {
                return enemyToKill;
            }

            set
            {
                enemyToKill = value;
            }
        }

        private int totalAmount; //hur många som ska dödas
        public int TotalAmountOfEnemies
        {
            get
            {
                return totalAmount;
            }
            set
            {
                totalAmount = value;
            }
        }

        private int currentAmount = 0; // hur många man har dödat
        public int CurrentAmountOfEnemiesKilled
        {
            get
            {
                return currentAmount;
            }

            private set
            {
                currentAmount = value;
            }
        }

        public KillQuest() { }

        public KillQuest(string name, Enemy enemyToKill, int totalAmountOfEnemies, int rewardMoney, Item rewardItem) : base(name, rewardMoney, rewardItem)
        {
            EnemyToKill = enemyToKill;
            TotalAmountOfEnemies = totalAmountOfEnemies;
        }

        public override int UpdateQuest() // uppdaterar questet 
        {
            if (TotalAmountOfEnemies != CurrentAmountOfEnemiesKilled)
                currentAmount++;

            return currentAmount;
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