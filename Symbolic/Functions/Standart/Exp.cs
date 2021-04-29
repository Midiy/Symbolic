using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Exp : Function
    {
        public Exp(Symbol variable) : base(variable) => HasAllIntegralsKnown = true;

        public override double GetValue(double variableValue) => Math.Exp(variableValue);

        public override Exp WithVariable(Symbol newVariable) => Exp(newVariable);

        public override bool Equals(Function? other) => other is Exp && other.Variable == Variable;

        public override string ToString(string inner) => $"exp({inner})";

        public override string ToPrefixString(string inner) => $"exp {inner}";

        protected override Exp _diff(Symbol _) => this;

        protected override Function _integrate(Symbol _) => this;
    }
}
