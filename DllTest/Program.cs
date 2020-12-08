using System;

using Symbolic.Functions;
using Symbolic.Functions.Standart;

namespace DllTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Symbol x = new Symbol("x");
            Function f = (new Cos().ApplyTo(new Ln()) + (x ^ 3)) * (x ^ 2) / new Power(2);
            Console.WriteLine(f);
            Console.ReadKey();
        }
    }
}
