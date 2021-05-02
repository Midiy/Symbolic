using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Cos : Function
    {
        public Cos(Function inner) : base(inner)
        {
            HasAllIntegralsKnown = true;
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"cos {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Cos(variableValue);

        protected override Function _applyTo(Function inner) => Cos(inner);

        protected override Function _diff(Symbol _) => -Sin(Inner);

        protected override Function _integrate(Symbol _) => Sin(Variable);

        protected override string _toString() => $"cos({Inner.ToString(PriorityWhenOuter)})";
    }
}
