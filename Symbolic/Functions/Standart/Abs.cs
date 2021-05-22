using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Abs : Partial
    {
        public Abs(Function inner) : base(new (Function, Constant, Constant)[] {
                                                                                   (-inner, Constant.NegativeInfinity, 0),
                                                                                   (inner, 0, Constant.PositiveInfinity)
                                                                               })
        {
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"abs {Inner.ToPrefixString()}";
            
        protected override double _getValue(double variableValue) => Math.Abs(variableValue);

        protected override Function _applyTo(Function inner) => Abs(inner);

        protected override string _toString() => $"|{Inner.ToString(PriorityWhenOuter)}|";
    }
}
