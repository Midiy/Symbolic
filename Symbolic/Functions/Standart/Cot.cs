using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Cot : Function
    {
        public Cot(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => 1 / Math.Tan(variableValue);

        public override Cot WithVariable(Symbol newVariable) => Cot(newVariable);

        public override bool Equals(Function? other) => other is Cot && other.Variable == Variable;

        public override string ToString(string inner) => $"cot({inner})";

        protected override Function _diff(Symbol _) => -1 / (Sin(Variable) ^ 2);

        protected override Function _integrate(Symbol _) => Ln(Abs(Sin(Variable)));
    }
}
