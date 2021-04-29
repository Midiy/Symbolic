using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Tan : Function
    {
        public Tan(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => Math.Tan(variableValue);

        public override Tan WithVariable(Symbol newVariable) => Tan(newVariable);

        public override bool Equals(Function? other) => other is Tan && other.Variable == Variable;

        public override string ToString(string inner) => $"tan({inner})";

        public override string ToPrefixString(string inner) => $"tan {inner}";

        protected override Function _diff(Symbol _) => 1 / (Cos(Variable) ^ 2);

        protected override Function _integrate(Symbol _) => -Ln(Abs(Cos(Variable)));
    }
}
