using System;

namespace Symbolic.Functions.Standart
{
    class Sqrt : Function
    {
        public override Quotient Diff() => (Quotient)(1 / (2 * new Sqrt()));

        public override double GetValue(double variableValue) => Math.Sqrt(variableValue);

        public override bool Equals(Function? other) => other is Sqrt;

        public override string ToString(string? inner) => $"sqrt({inner})";
    }
}
