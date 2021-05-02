using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Tan : Function
    {
        public Tan(Function inner) : base(inner)
        {
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"tan {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Tan(variableValue);

        protected override Function _applyTo(Function inner) => Tan(inner);

        protected override Function _diff(Symbol _) => 1 / (Cos(Inner) ^ 2);

        protected override Function _integrate(Symbol _) => -Ln(Abs(Cos(Variable)));

        protected override string _toString() => $"tan({Inner.ToString(PriorityWhenOuter)})";
    }
}
