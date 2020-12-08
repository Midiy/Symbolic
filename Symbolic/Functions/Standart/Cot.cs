using System;

namespace Symbolic.Functions.Standart
{
    public class Cot : Function
    {
        public override Quotient Diff() => (Quotient)(-1 / (new Sin() ^ 2));

        public override double GetValue(double variableValue) => 1 / Math.Tan(variableValue);

        public override bool Equals(Function? other) => other is Cot;

        public override string ToString(string? inner) => $"cot({inner})";
    }
}
