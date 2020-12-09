using System;

namespace Symbolic.Functions.Standart
{
    public class Ln : Function
    {
        public Ln(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => Math.Log(variableValue);

        public override Ln WithVariable(Symbol newVariable) => new Ln(newVariable);

        public override bool Equals(Function? other) => other is Ln && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"ln({inner})";

        protected override Function _diff(Symbol variable) => 1 / Variable!;
    }
}
