using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon.Models.Items.Equipment
{
    abstract class Equipment : Item
    {

        protected Equipment(string name, int price) : base (name, price)
        {
            Equiptable = true;            
        }
    }
}
