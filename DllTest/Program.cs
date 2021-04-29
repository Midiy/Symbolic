using System;

using Symbolic.Functions;
using Symbolic.Functions.Standart;

using static Symbolic.Utils.FunctionFactory;

namespace DllTest
{
    class Program
    {
        static void Main()
        {
            Symbol x = Symbol("x");
            Symbol y = Symbol("y");

            Console.WriteLine(new Sin(x).GetHashCode() == new Sin(x).GetHashCode());
            Console.WriteLine(new Sin(x).GetHashCode() == Sin(x).GetHashCode());
            Console.WriteLine(new Sin(x).GetHashCode() == Sin("x").GetHashCode());
            Console.WriteLine(new Sin(x).GetHashCode() == new Sin(y).GetHashCode());
            Console.WriteLine((3 * (x ^ 2) - x + 5).GetHashCode() == new Polynomial(x, 3, -1, 5).GetHashCode());
            Console.WriteLine((3 * (x ^ 2) - x + 5).GetHashCode() == new Polynomial(x, 3, -1, 4).GetHashCode());
            Console.WriteLine();

            // It is recommended to use FunctionFactory methods because of the results caching
            // (which, however, could be disabled by setting Symbolic.Utils.Properties.UseCaching to false).
            Console.WriteLine(object.ReferenceEquals(Cos(x), Cos("x")));
            Console.WriteLine(object.ReferenceEquals(Cos(Symbolic.Functions.Symbol.ANY).ApplyTo(Sin(x)), Cos(Sin(x))));
            Console.WriteLine(object.ReferenceEquals(3 * (x ^ 2) - x + 5, Polynomial(x, 3, -1, 5)));
            Console.WriteLine(object.ReferenceEquals(new Cos(x), Cos(x)));
            Console.WriteLine();

            Function fi = ((x ^ 3) * Sin(x)).Integrate(x);
            Console.WriteLine(fi);
            Console.WriteLine();

            Function f1 = (Cos(Ln(x)) + (x ^ 3)) * (x ^ 2) / Power(x, 2);
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
            f3 += Monomial(x, 3, 2);
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
            Console.WriteLine();

            Console.WriteLine(fi.ToPrefixString());
            Console.WriteLine(f1.ToPrefixString());
            Console.WriteLine(f2.ToPrefixString());
            Console.WriteLine(f3.ToPrefixString());
        }
    }
}
