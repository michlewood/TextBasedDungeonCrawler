using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    abstract class Weapon : Equipment
    {
        public int DamageDone { get; protected set; }
        
        public override string StorDescription()
        {
            return Name + " - Price: " + Price + "gp -  a weapon that deals " + DamageDone + " + amount of damage";
        }

    }

    internal class Dagger : Weapon
    {
        internal Dagger()
        {
            Name = "Dagger";
            Price = 15;
            DamageDone = 20;
            Equiptable = true;
            IsWeapon = true;
        }
    }
}
