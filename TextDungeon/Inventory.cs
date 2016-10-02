using System;
using System.Collections.Generic;

namespace TextDungeon
{
    internal class Inventory
    {
        static private List<Item> itemList = new List<Item>();
        internal List<Item> ItemList
        {
            get
            {
                List<Item> tempInventory = new List<Item>();
                tempInventory = itemList;
                return tempInventory;

            }
        }

        internal void AddItem(Item item)
        {
            if (itemList.Find(x => x.Name == item.Name) != null)
            {
                itemList.Insert(ItemList.FindLastIndex(x => x.Name == item.Name), item);

            }

            else itemList.Add(item);
        }

        internal void RemoveItem(Item item)
        {
            itemList.Remove(item);
        }
    }
}