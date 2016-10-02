﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDungeon
{

    class Printer
    {

        private static string history;
        public string History
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

        private static string roomHistory;
        public string RoomHistory
        {
            get
            {
                return roomHistory;
            }
            private set
            {
                roomHistory = value;
            }
        }

        internal void PrintLine(string input, params object[] arg) //(Används oftast istället för Console.WriteLine) skriver ut det stringen som är i stringen och sparar det till historia så att det kan skrivas igen 
        {
            string remadeInput = AddArgToString(input, arg);

            RoomHistory += History = remadeInput + "\n";
            Console.WriteLine(remadeInput);
        }

        internal void Print(string input, params object[] arg) //(Används oftast istället för Console.WriteLine) skriver ut det stringen som är i stringen och sparar det till historia så att det kan skrivas igen 
        {
            string remadeInput = AddArgToString(input, arg);

            RoomHistory += History = remadeInput;
            Console.Write(remadeInput);
        }

        private string AddArgToString(string input, params object[] arg)
        {
            int numberBetweenBrackets;
            string remadeInput = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '{' && int.TryParse("" + input[i + 1], out numberBetweenBrackets) && input[i + 2] == '}')
                {
                    remadeInput += "" + arg[numberBetweenBrackets];
                    i += 2;
                }

                else remadeInput += "" + input[i];

            }

            return remadeInput;
        }

        internal string Reader() // (Används oftast iställer för Console.ReadLine) tar emot input från spelaren och returnerar det som ett string och sparar det till historia 
        {
            string input = Console.ReadLine();

            RoomHistory += History = input + "\n";

            return input;

        }

        internal void ClearRoomHistory()
        {
            RoomHistory = "";
        }

    }
}