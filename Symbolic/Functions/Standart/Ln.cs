using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Ln : Log
    {
        public Ln(Function inner) : base(inner, Constant.E) { }

        public override string ToPrefixString() => $"ln {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Log(variableValue);

        protected override Function _applyTo(Function inner) => Ln(inner);

        protected override Function _diff(Symbol _) => 1 / Inner;

        protected override Function _integrate(Symbol _) => Variable * Ln(Variable) - Variable;

        protected override string _toString() => $"ln({Inner.ToString(PriorityWhenOuter)})";
    }
}
