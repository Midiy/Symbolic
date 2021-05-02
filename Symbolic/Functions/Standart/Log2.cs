using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Log2 : Log
    {
        public Log2(Function inner) : base(inner, 2) { }

        public override string ToPrefixString() => $"log2 {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Log2(variableValue);

        protected override Function _applyTo(Function inner) => Log2(inner);

        protected override string _toString() => $"log2({Inner.ToString(PriorityWhenOuter)})";
    }
}
