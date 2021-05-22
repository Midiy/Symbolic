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
            Console.WriteLine($"{ReferenceEquals(sin1, sin2)} (expected to be False)");
            Console.WriteLine($"{ReferenceEquals(sin2, sin3)} (expected to be False)");
            Console.WriteLine($"{ReferenceEquals(sin1, sin3)} (expected to be True)");
            Console.WriteLine();

            // HashCode demonstration.
            Console.WriteLine($"{new Sin(x).GetHashCode() == new Sin(x).GetHashCode()} (expected to be True)");
            Console.WriteLine($"{new Sin(x).GetHashCode() == Sin(x).GetHashCode()} (expected to be True)");
            Console.WriteLine($"{new Sin(x).GetHashCode() == Sin("x").GetHashCode()} (expected to be True)");
            Console.WriteLine($"{new Sin(x).GetHashCode() == new Sin(y).GetHashCode()} (expected to be False)");
            Console.WriteLine($"{(3 * (x ^ 2) - x + 5).GetHashCode() == new Polynomial(x, 3, -1, 5).GetHashCode()} (expected to be True)");
            Console.WriteLine($"{(3 * (x ^ 2) - x + 5).GetHashCode() == new Polynomial(x, 3, -1, 4).GetHashCode()} (expected to be False)");
            Console.WriteLine();

            // Caching demonstration.
            // It is recommended to use FunctionFactory methods because of the results caching
            // (which, however, could be disabled by setting Symbolic.Utils.Properties.UseCaching to false).
            Console.WriteLine($"{ReferenceEquals(Cos(x), Cos("x"))} (expected to be True)");
            Console.WriteLine($"{ReferenceEquals(Cos(Symbolic.Functions.Symbol.ANY).ApplyTo(Sin(x)), Cos(Sin(x)))} (expected to be True)");
            Console.WriteLine($"{ReferenceEquals(3 * (x ^ 2) - x + 5, Polynomial(x, 3, -1, 5))} (expected to be True)");
            Console.WriteLine($"{ReferenceEquals(new Cos(x), Cos(x))} (expected to be False)");
            Console.WriteLine();

            // Integration demonstration.
            Function fi = ((x ^ 3) * Sin(x)).Integrate(x);
            Console.WriteLine($"{fi} (expected to be -x^3 * cos(x) + 3x^2 * sin(x) + 6x * cos(x) - 6 * sin(x))");
            Console.WriteLine();

            // Simplification, differentiation and evaluation demonstration.
            Function f1 = (Cos(Ln(x)) + (x ^ 3)) * (x ^ 2) / Power(x, 2);
            Console.WriteLine($"{f1} (expected to be cos(ln(x)) + x^3)");
            Console.WriteLine($"{f1.Diff(x)} (expected to be -sin(ln(x)) * 1 / x + 3x^2)");
            Console.WriteLine($"{f1.Diff(y)} (expected to be 0)");
            Console.WriteLine($"{f1.GetValue(5)} (expected to be {Math.Cos(Math.Log(5)) + Math.Pow(5, 3)})");
            Console.WriteLine();

            // Differentiation with respect to different variables demonstration.
            Function f2 = (x ^ 2) + (y ^ 2);
            Console.WriteLine($"{f2.Diff(x)} (expected to be 2x)");
            Console.WriteLine($"{f2.Diff(y)} (expected to be 2y)");
            Console.WriteLine();

            // Polynomial transformations demonstration.
            Function f3 = (x ^ 3) - 3 * (x ^ 2) + 3 * x - 1;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be x^3 - 3x^2 + 3x - 1, True)");
            f3 += 5;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be x^3 - 3x^2 + 3x + 4, True)");
            f3 += x;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be x^3 - 3x^2 + 4x + 4, True)");
            f3 -= 3 * x;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be x^3 - 3x^2 + x + 4, True)");
            f3 += Monomial(x, 3, 2);
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be x^3 + x + 4, True)");
            f3 *= -1;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be -x^3 - x - 4, True)");
            f3 *= x;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be -x^4 - x^2 - 4x, True)");
            f3 *= -x;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be x^5 + x^3 + 4x^2, True)");
            Function f_diff = f3.Diff(x);
            Console.WriteLine($"{f_diff}, {f_diff is Polynomial} (expected to be 5x^4 + 3x^2 + 8x, True)");
            f3 *= y;
            Console.WriteLine($"{f3}, {f3 is Polynomial} (expected to be (x^5 + x^3 + 4x^2) * y, False)");
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
