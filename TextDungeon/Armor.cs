namespace TextDungeon
{
    abstract class Armor : Equipment
    {

        public int ArmorRating { get; protected set; }

        public override string StorDescription()
        {
            return Name + " - Price: " + Price + "gp - Wearable armor with an armorvalue of " + ArmorRating;
        }

    }

    internal class Shirt : Armor
    {
        internal Shirt()
        {
            Name = "Shirt";
            Price = 1;
            Equiptable = true;
            ArmorRating = 45;
            IsArmor = true;
        }



    }
}