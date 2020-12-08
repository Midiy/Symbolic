using System;

namespace Symbolic.Functions.Standart
{
    public class Ln : Function
    {
        public Ln(Symbol variable) : base(variable) { }

        public override Quotient Diff() => (Quotient)(1 / Variable!);

        public override double GetValue(double variableValue) => Math.Log(variableValue);

        public override bool Equals(Function? other) => other is Ln && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"ln({inner})";
    }
}
