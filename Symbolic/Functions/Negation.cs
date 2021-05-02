using Symbolic.Utils;

namespace Symbolic.Functions
{
    public class Negation : Function
    {
        public Negation(Function inner) : base(inner)
        {
            HasAllIntegralsKnown = inner.HasAllIntegralsKnown;
            PriorityWhenInner = Priorities.InnerNegation;
            PriorityWhenOuter = Priorities.OuterNegation;
        }

        public override Function Integrate(Symbol variable)
        {
            if (variable == Variable) { return -Inner.Integrate(variable); }
            else { return this * variable; }
        }

        public override Function Negate() => Inner;

        public override Function Multiply(Function other)
        {
            if (other is Negation n) { return Inner.Multiply(n.Inner); }
            else { return base.Multiply(other); }
        }

        public override Function Divide(Function other)
        {
            if (other is Negation n) { return Inner.Divide(n.Inner); }
            else { return base.Divide(other); }
        }

        public override string ToPrefixString() => $"* ( -1 ) {Inner.ToPrefixString()}";   // To avoid ambiguity with unary and binary minus.

        protected override double _getValue(double variableValue) => -variableValue;

        protected override Function _applyTo(Function inner) => -inner;

        protected override Function _diff(Symbol _) => -1;

        protected override Function _integrate(Symbol variable) => Integrate(variable);

        protected override string _toString() => $"-{Inner.ToString(PriorityWhenOuter)}";
    }
}
