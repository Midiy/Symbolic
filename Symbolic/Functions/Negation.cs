﻿namespace Symbolic.Functions
{
    public class Negation : Function
    {
        public Function Inner { get; init; }

        public Negation(Function inner) : base(inner.Variable)
        {
            Inner = inner;
            HasAllIntegralsKnown = inner.HasAllIntegralsKnown;
        }

        public override double GetValue(double variableValue) => -Inner.GetValue(variableValue);

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

        public override Function ApplyTo(Function inner) => -Inner.ApplyTo(inner);

        public override Function WithVariable(Symbol newVariable) => -Inner.WithVariable(newVariable);

        public override bool Equals(Function? other) => other is Negation n && n.Inner == Inner;

        public override string ToString(string inner) => $"-({Inner.ToString(inner)})";

        public override string ToString() => $"-({Inner})";

        protected override Function _diff(Symbol variable) => -Inner.Diff(variable);

        protected override Function _integrate(Symbol variable) => -Inner.Integrate(variable);
    }
}
