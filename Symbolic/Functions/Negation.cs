﻿namespace Symbolic.Functions
{
    public class Negation : Function
    {
        public Function Inner { get; init; }

        public Negation(Function inner) => Inner = inner;

        public override Function Diff() => -Inner.Diff();

        public override double GetValue(double variableValue) => -Inner.GetValue(variableValue);

        public override Function Negate() => Inner;

        public override Function ApplyTo(Function inner) => -Inner.ApplyTo(inner);

        public override bool Equals(Function? other) => other is Negation n && n.Inner == Inner;

        public override string ToString(string? inner) => $"-({Inner.ToString(inner)})";
    }
}