using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Cot : Function
    {
        public Cot(Function inner) : base(inner)
        {
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"cot {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => 1 / Math.Tan(variableValue);

        protected override Function _applyTo(Function inner) => Cot(inner);

        protected override Function _diff(Symbol _) => -1 / (Sin(Inner) ^ 2);

        protected override Function _integrate(Symbol _) => Ln(Abs(Sin(Variable)));

        protected override string _toString() => $"cot({Inner.ToString(PriorityWhenOuter)})";
    }
}
