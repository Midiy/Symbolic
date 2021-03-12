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

            Function fi = ((x ^ 3) * new Sin(x)).Integrate(x);
            Console.WriteLine(fi);
            Console.WriteLine();


            Function f1 = (new Cos(x).ApplyTo(new Ln(x)) + (x ^ 3)) * (x ^ 2) / new Power(x, 2);
            Console.WriteLine(f1);
            Console.WriteLine(f1.Diff(x));
            Console.WriteLine(f1.Diff(y));
            Console.WriteLine($"{f1.GetValue(5)} (expected: {Math.Cos(Math.Log(5)) + Math.Pow(5, 3)})");
            Console.WriteLine();

            Function f2 = (x ^ 2) + (y ^ 2);
            Console.WriteLine(f2.Diff(x));
            Console.WriteLine(f2.Diff(y));
            Console.WriteLine();

            Function f3 = (x ^ 3) - 3 * (x ^ 2) + 3 * x - 1;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 += 5;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 += x;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 -= 3 * x;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 += new Monomial(x, 3, 2);
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 *= -1;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 *= x;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            f3 *= -x;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            Console.WriteLine(f3.Diff(x));
            f3 *= y;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
        }
    }
}
