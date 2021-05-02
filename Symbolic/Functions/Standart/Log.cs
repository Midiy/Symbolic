using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Log : Function
    {
        public Constant Base { get; init; }

        public Log(Function inner, Constant @base) : base(inner)
        {
            Base = @base;
            HasAllIntegralsKnown = true;
            PriorityWhenInner = Priorities.InnerStandartFunctions;
            PriorityWhenOuter = Priorities.OuterStandartFunctions;
        }

        public override string ToPrefixString() => $"log {Inner.ToPrefixString()} {Base.ToPrefixString()}";

        protected override double _getValue(double variableValue) => Math.Log(variableValue) / Math.Log(Base);

        protected override Function _applyTo(Function inner) => Log(inner, Base);

        protected override bool _equals(Function other) => other is Log l && Base == l.Base;

        protected override Function _diff(Symbol _) => 1 / (Ln(Base) * Inner);

        protected override Function _integrate(Symbol _) => (Variable * Ln(Variable) - Variable) / Ln(Base);

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.Add(Base);

        protected override string _toString() => $"log({Inner.ToString(PriorityWhenOuter)}, {Base.ToString(PriorityWhenOuter)})";
    }
}
