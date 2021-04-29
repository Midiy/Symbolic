using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Sqrt : Root
    {
        public Sqrt(Symbol variable) : base(variable, 2) { }

        public override double GetValue(double variableValue) => Math.Sqrt(variableValue);

        public override Sqrt WithVariable(Symbol newVariable) => Sqrt(newVariable);

        public override string ToString(string inner) => $"sqrt({inner})";

        public override string ToPrefixString(string inner) => $"sqrt {inner}";

        protected override Function _diff(Symbol _) => 1 / (2 * Sqrt(Variable));

        protected override Function _integrate(Symbol _) => 2 * Variable * Sqrt(Variable);
    }
}
