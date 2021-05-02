using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Exp : Function
    {
        public Exp(Function inner) : base(inner)
        {
            HasAllIntegralsKnown = true;
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"exp {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Exp(variableValue);

        protected override Function _applyTo(Function inner) => Exp(inner);

        protected override Exp _diff(Symbol _) => this;

        protected override Function _integrate(Symbol _) => this;

        protected override string _toString() => $"exp({Inner.ToString(PriorityWhenOuter)})";
    }
}
