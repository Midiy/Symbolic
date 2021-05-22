using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Power : Function
    {
        public Constant Exponent { get; init; }

        public Power(Function inner, Constant exponent) : base(inner)
        {
            Exponent = exponent;
            HasAllIntegralsKnown = true;
            PriorityWhenInner = Priorities.Exponentiation;
            PriorityWhenOuter = Priorities.Exponentiation;
        }

        public override string ToPrefixString() => $"^ {Inner.ToPrefixString()} {Exponent.ToPrefixString()}";

        public override Function Add(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Power p && Exponent == p.Exponent && Exponent >= 0 && Exponent % 1 == 0) { return Monomial(Variable, 2, Exponent); }
                else if (other is Monomial m && Exponent == m.Exponent) { return m.Add(this); }
            }
            return base.Add(other);
        }

        public override Function Subtract(Function other)
        {
            if (other is Monomial m && Variable == m.Variable && Exponent == m.Exponent) { return m.Add(this); }
            else { return base.Add(other); }
        }

        protected override double _getValue(double variableValue) => Math.Pow(variableValue, Exponent);

        protected override Function _applyTo(Function inner) => Power(inner, Exponent);

        protected override bool _equals(Function other) => (other is Power p && Exponent == p.Exponent) || (other is Monomial && other == (Monomial)this);

        protected override Function _diff(Symbol _) => Exponent * (Inner ^ (Exponent - 1));

        protected override Function _integrate(Symbol _) => (Variable ^ (Exponent + 1)) / (Exponent + 1);

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.Add(Exponent);

        protected override string _toString() => $"{Inner.ToString(PriorityWhenOuter)}^{Exponent.ToString(PriorityWhenOuter)}";
    }
}
