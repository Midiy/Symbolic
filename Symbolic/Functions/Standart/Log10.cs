using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Log10 : Log
    {
        public Log10(Function inner) : base(inner, 10) { }

        public override string ToPrefixString() => $"log10 {Inner.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Log10(variableValue);

        protected override Function _applyTo(Function inner) => Log10(inner);

        protected override string _toString() => $"log10({Inner.ToString(PriorityWhenOuter)})";
    }
}
