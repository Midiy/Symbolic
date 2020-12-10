using System;

namespace Symbolic.Functions.Standart
{
    public class Exp : Function
    {
        public Exp(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => Math.Exp(variableValue);

        public override Exp WithVariable(Symbol newVariable) => new Exp(newVariable);

        public override bool Equals(Function? other) => other is Exp && other.Variable! == Variable!;

        public override string ToString(string inner) => $"exp({inner})";

        protected override Exp _diff(Symbol variable) => this;
    }
}
