using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{
    abstract class Weapon : Equipment
    {
        protected Weapon(string name, int price, int damageDone) : base(name, price)
        {
            DamageDone = damageDone;
            IsWeapon = true;            
        }

        public int DamageDone { get; protected set; }
        
        public override string StorDescription()
        {
            return Name + " - Price: " + Price + "gp -  a weapon that deals " + DamageDone + " + amount of damage";
        }

    }

    internal class Dagger : Weapon
    {
        internal Dagger() : base("Dagger", 15, 20)
        {

        }
    }
}
