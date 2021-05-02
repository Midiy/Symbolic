using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Sqrt : Root
    {
        public Sqrt(Function inner) : base(inner, 2)
        {
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"sqrt {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Sqrt(variableValue);

        protected override Function _applyTo(Function inner) => Sqrt(inner);

        protected override Function _diff(Symbol _) => 1 / (2 * Sqrt(Inner));

        protected override Function _integrate(Symbol _) => 2 / 3 * Variable * Sqrt(Variable);

        protected override string _toString() => $"sqrt({Inner.ToString(PriorityWhenOuter)})";
    }
}
