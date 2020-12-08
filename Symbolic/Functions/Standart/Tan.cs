using System;

namespace Symbolic.Functions.Standart
{
    public class Tan : Function
    {
        public override Quotient Diff() => (Quotient)(1 / (new Cos() ^ 2));

        public override double GetValue(double variableValue) => Math.Tan(variableValue);

        public override bool Equals(Function? other) => other is Tan;

        public override string ToString(string? inner) => $"tan({inner})";
    }
}
