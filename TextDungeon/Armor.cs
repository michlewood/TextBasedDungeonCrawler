namespace TextDungeon
{
    abstract class Armor : Equipment
    {

        public int ArmorRating { get; protected set; }

        protected Armor(string name, int price, int armorRating) : base (name, price)
        {
            IsArmor = true;
            ArmorRating = armorRating;
        }

        public override string StorDescription()
        {
            return Name + " - Price: " + Price + "gp - Wearable armor with an armorvalue of " + ArmorRating;
        }

    }

    internal class Shirt : Armor
    {
        internal Shirt() : base ("Shirt", 1, 45)
        {

        }
    }
}