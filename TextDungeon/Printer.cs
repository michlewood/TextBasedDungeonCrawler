using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{

    class Printer
    {

        private static string history;
        public static string History
        {
            get
            {
                return history;
            }
            private set
            {
                history += value;
            }
        }

        public static string RoomHistory { get; private set; }

        public static void PrintLine(string input, params object[] arg) //(Används oftast istället för Console.WriteLine) skriver ut det stringen som är i stringen och sparar det till historia så att det kan skrivas igen 
        {
            string remadeInput = AddArgToString(input, arg);

            RoomHistory += History = remadeInput + "\n";
            Console.WriteLine(remadeInput);
        }

        public static void Print(string input, params object[] arg) //(Används oftast istället för Console.WriteLine) skriver ut det stringen som är i stringen och sparar det till historia så att det kan skrivas igen 
        {
            string remadeInput = AddArgToString(input, arg);

            RoomHistory += History = remadeInput;
            Console.Write(remadeInput);
        }

        private static string AddArgToString(string input, params object[] arg)
        {
            int numberBetweenBrackets;
            string remadeInput = "";

            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '{' && int.TryParse("" + input[i + 1], out numberBetweenBrackets) && input[i + 2] == '}')
                    {
                        remadeInput += "" + arg[numberBetweenBrackets];
                        i += 2;
                    }

                    else remadeInput += "" + input[i];

                }
            }
            return remadeInput;
        }

        public static string Reader() // (Används oftast iställer för Console.ReadLine) tar emot input från spelaren och returnerar det som ett string och sparar det till historia 
        {
            string input = Console.ReadLine();

            RoomHistory += History = input + "\n";

            return input;

        }

        public static void ClearRoomHistory()
        {
            RoomHistory = "";
        }

        public static string ReadKey(bool intercept = false)
        {
            string input = Console.ReadKey(intercept).Key.ToString();
            if (intercept) input = "\n";

            RoomHistory += History = input;

            return input;
        }
    }
}
