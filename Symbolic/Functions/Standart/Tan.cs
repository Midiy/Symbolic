using System;

namespace Symbolic.Functions.Standart
{
    public class Tan : Function
    {
        public Tan(Symbol variable) : base(variable) { }

        public override Quotient Diff() => (Quotient)(1 / (new Cos(Variable!) ^ 2));

        public override double GetValue(double variableValue) => Math.Tan(variableValue);

        public override bool Equals(Function? other) => other is Tan && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"tan({inner})";
    }
}
