using System;

namespace TextDungeon
{
    public class Quest
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
        public int TotalAmount
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

        private int currentAmount; // hur många man har dödat
        public int CurrentAmount
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

        internal Quest(string name, Enemy enemyToKill, int totalAmount, int rewardMoney, Item rewardItem) //konstruktor för quest (om man t ex inte vill att man ska få ett item skriv null) 
        {
            Name = name;
            EnemyToKill = enemyToKill;
            TotalAmount = totalAmount;
            CurrentAmount = 0;
            RewardMoney = rewardMoney;
            RewardItem = rewardItem;
        }

        public int UpdateQuest() // uppdaterar questet 
        {


            if (TotalAmount != CurrentAmount)
                currentAmount++;

            return currentAmount;

        }

        internal void ResetQuest()
        {
            CurrentAmount = 0;
        }

        internal bool CheckCompleted() // kollar om målet i questen är avklarat 
        {
            if (CurrentAmount == TotalAmount) return true;
            else return false;
        }
    }
}