using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Sin : Function
    {
        public Sin(Symbol variable) : base(variable) => HasAllIntegralsKnown = true;

        public override double GetValue(double variableValue) => Math.Sin(variableValue);

        public override Sin WithVariable(Symbol newVariable) => Sin(newVariable);

        public override bool Equals(Function? other) => other is Sin && other.Variable == Variable;

        public override string ToString(string inner) => $"sin({inner})";

        public override string ToPrefixString(string inner) => $"sin {inner}";

        protected override Cos _diff(Symbol _) => Cos(Variable);

        protected override Function _integrate(Symbol _) => -Cos(Variable);
    }
}
