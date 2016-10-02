using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TextDungeon
{
    internal class Win
    {
        private volatile bool stopWinLoop;

        public void WinScreen()
        {
            while (!stopWinLoop)
            {
                Console.Clear();
                if (Console.ForegroundColor == ConsoleColor.Green) Console.ForegroundColor = ConsoleColor.Blue;
                else Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("You Win!".PadLeft(65));
                Thread.Sleep(600);
            }
        }

        public void Stop()
        {
            stopWinLoop = true;
        }

    }
}