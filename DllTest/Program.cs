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
            Symbol y = new Symbol("y");

            Function f1 = (new Cos(x).ApplyTo(new Ln(x)) + (x ^ 3)) * (x ^ 2) / new Power(x, 2);
            Console.WriteLine(f1);
            Console.WriteLine(f1.Diff(x));
            Console.WriteLine(f1.Diff(y));
            Console.WriteLine($"{f1.GetValue(5)} (expected: {Math.Cos(Math.Log(5)) + Math.Pow(5, 3)})");

            Function f2 = (x ^ 2) + (y ^ 2);
            Console.WriteLine(f2.Diff(x));
            Console.WriteLine(f2.Diff(y));
        }
    }
}
