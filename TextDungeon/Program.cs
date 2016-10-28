using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TextDungeon
{
    class Program
    {
        static void Main(string[] args) //Main metoden anropar bara konstruktorn 
        {
            Runtime runtime = new Runtime();
            runtime.Start();
        }
    }
}
