using System;

using Symbolic.Functions;
using Symbolic.Functions.Standart;
using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace DllTest
{
    class Program
    {
        static void Main()
        {
            Symbol x = Symbol("x");
            Symbol y = Symbol("y");

            // Properties.WithoutCaching() usage demonstration.
            Function sin1, sin2, sin3;
            sin1 = Sin(x);
            using (new Properties.WithoutCaching())
            {
                sin2 = Sin(x);
            }
            sin3 = Sin(x);
            Console.WriteLine(ReferenceEquals(sin1, sin2));
            Console.WriteLine(ReferenceEquals(sin2, sin3));
            Console.WriteLine(ReferenceEquals(sin1, sin3));
            Console.WriteLine();

            // HashCode demonstration.
            Console.WriteLine(new Sin(x).GetHashCode() == new Sin(x).GetHashCode());
            Console.WriteLine(new Sin(x).GetHashCode() == Sin(x).GetHashCode());
            Console.WriteLine(new Sin(x).GetHashCode() == Sin("x").GetHashCode());
            Console.WriteLine(new Sin(x).GetHashCode() == new Sin(y).GetHashCode());
            Console.WriteLine((3 * (x ^ 2) - x + 5).GetHashCode() == new Polynomial(x, 3, -1, 5).GetHashCode());
            Console.WriteLine((3 * (x ^ 2) - x + 5).GetHashCode() == new Polynomial(x, 3, -1, 4).GetHashCode());
            Console.WriteLine();

            // Caching demonstration.
            // It is recommended to use FunctionFactory methods because of the results caching
            // (which, however, could be disabled by setting Symbolic.Utils.Properties.UseCaching to false).
            Console.WriteLine(ReferenceEquals(Cos(x), Cos("x")));
            Console.WriteLine(ReferenceEquals(Cos(Symbolic.Functions.Symbol.ANY).ApplyTo(Sin(x)), Cos(Sin(x))));
            Console.WriteLine(ReferenceEquals(3 * (x ^ 2) - x + 5, Polynomial(x, 3, -1, 5)));
            Console.WriteLine(ReferenceEquals(new Cos(x), Cos(x)));
            Console.WriteLine();

            // Integration demonstration.
            Function fi = ((x ^ 3) * Sin(x)).Integrate(x);
            Console.WriteLine(fi);
            Console.WriteLine();

            // Simplification, differentiation and evaluation demonstration.
            Function f1 = (Cos(Ln(x)) + (x ^ 3)) * (x ^ 2) / Power(x, 2);
            Console.WriteLine(f1);
            Console.WriteLine(f1.Diff(x));
            Console.WriteLine(f1.Diff(y));
            Console.WriteLine($"{f1.GetValue(5)} (expected: {Math.Cos(Math.Log(5)) + Math.Pow(5, 3)})");
            Console.WriteLine();

            // Differentiation with respect to different variables demonstration.
            Function f2 = (x ^ 2) + (y ^ 2);
            Console.WriteLine(f2.Diff(x));
            Console.WriteLine(f2.Diff(y));
            Console.WriteLine();

            // Polynomial transformations demonstration.
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
            Function f_diff = f3.Diff(x);
            Console.WriteLine($"{f_diff}, {f_diff is Polynomial}");
            f3 *= y;
            Console.WriteLine($"{f3}, {f3 is Polynomial}");
            Console.WriteLine();

            // Prefix representation demonstration.
            Console.WriteLine($"{fi}\n" +
                              $"     |\n" +
                              $"     V\n" +
                              $"{fi.ToPrefixString()}\n");
            Console.WriteLine($"{f1}\n" +
                              $"     |\n" +
                              $"     V\n" +
                              $"{f1.ToPrefixString()}\n");
            Console.WriteLine($"{f2}\n" +
                              $"     |\n" +
                              $"     V\n" +
                              $"{f2.ToPrefixString()}\n");
            Console.WriteLine($"{f3}\n" +
                              $"     |\n" +
                              $"     V\n" +
                              $"{f3.ToPrefixString()}\n");
        }
    }
}
