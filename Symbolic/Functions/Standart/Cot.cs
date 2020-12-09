using System;

namespace Symbolic.Functions.Standart
{
    public class Cot : Function
    {
        public Cot(Symbol variable) : base(variable) { }

        public override Quotient Diff() => (Quotient)(-1 / (new Sin(Variable!) ^ 2));

        public override double GetValue(double variableValue) => 1 / Math.Tan(variableValue);

        public override Cot WithVariable(Symbol newVariable) => new Cot(newVariable);

        public override bool Equals(Function? other) => other is Cot && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"cot({inner})";
    }
}
