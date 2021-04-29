﻿using Symbolic.Utils;

namespace Symbolic.Functions
{
    public class Quotient : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Quotient(Function left, Function right)
        {
            if (right is Constant c && c.Value == 0) { throw new System.DivideByZeroException(); }
            (Left, Right) = (left, right);
            if (Left.Variable == Right.Variable) { Variable = Left.Variable | Right.Variable; }
            if (Right is Constant) { HasAllIntegralsKnown = Left.HasAllIntegralsKnown; }
        }

        public override double GetValue(double variableValue) => Left.GetValue(variableValue) / Right.GetValue(variableValue);

        public override Function Multiply(Function other) => other == Right ? Left : base.Multiply(other);

        public override Function Divide(Function other) => other is Quotient q ? (Left * q.Right) / (Right * q.Left) : Left / (Right * other);

        public override Function ApplyTo(Function inner) => Left.ApplyTo(inner) / Right.ApplyTo(inner);

        public override Function WithVariable(Symbol newVariable) => Left.WithVariable(newVariable) / Right.WithVariable(newVariable);

        public override bool Equals(Function? other) => other is Quotient q && q.Left == Left && q.Right == Right;

        public override string ToString(string inner) => $"({Left.ToString(inner)}) / ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) / ({Right})";

        public override string ToPrefixString(string inner) => $"/ {Left.ToPrefixString(inner)} {Right.ToPrefixString(inner)}";

        public override string ToPrefixString() => $"/ {Left.ToPrefixString()} {Right.ToPrefixString()}";

        protected override Function _diff(Symbol variable) => (Left.Diff(variable) * Right - Right.Diff(variable) * Left) / (Right ^ 2);

        protected override Function _integrate(Symbol variable)
        {
            if (Right is Constant c) { return Left.Integrate(variable) / c; }
            else { throw new System.NotImplementedException(); }
        }

        protected override HashCodeCombiner _addHashCodeVariable(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner.Add(Left).Add(Right);
    }
}
