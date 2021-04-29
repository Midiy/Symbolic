using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Ln : Log
    {
        public Ln(Symbol variable) : base(variable, Constant.E) { }

        public override double GetValue(double variableValue) => Math.Log(variableValue);

        public override Ln WithVariable(Symbol newVariable) => Ln(newVariable);

        public override string ToString(string inner) => $"ln({inner})";

        public override string ToPrefixString(string inner) => $"ln {inner}";

        protected override Function _diff(Symbol _) => 1 / Variable;

        protected override Function _integrate(Symbol _) => Variable * Ln(Variable) - Variable;
    }
}
