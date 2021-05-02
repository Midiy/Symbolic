using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Sin : Function
    {
        public Sin(Function inner) : base(inner)
        {
            HasAllIntegralsKnown = true;
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"sin {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Sin(variableValue);

        protected override Function _applyTo(Function inner) => Sin(inner);

        protected override Function _diff(Symbol _) => Cos(Inner);

        protected override Function _integrate(Symbol _) => -Cos(Variable);

        protected override string _toString() => $"sin({Inner.ToString(PriorityWhenOuter)})";
    }
}
