using Symbolic.Utils;

namespace Symbolic.Functions
{
    public class Sum : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Sum(Function left, Function right)
        {
            (Left, Right) = (left, right);
            if (Left.Variable == Right.Variable) { Variable = Left.Variable | Right.Variable; }
            HasAllIntegralsKnown = Left.HasAllIntegralsKnown && Right.HasAllIntegralsKnown;
        }

        public override double GetValue(double variableValue) => Left.GetValue(variableValue) + Right.GetValue(variableValue);

        public override Function Subtract(Function other)
        {
            if (other == Left) { return Right; }
            else if (other == Right) { return Left; }
            else if (other is Sum s)
            {
                if (s.Left == Left) { return Right.Subtract(s.Right); }
                else if (s.Left == Right) { return Left.Subtract(s.Right); }
                else if (s.Right == Left) { return Right.Subtract(s.Left); }
                else if (s.Right == Right) { return Left.Subtract(s.Left); }
            }
            return base.Subtract(other);
        }

        public override Function ApplyTo(Function inner) => Left.ApplyTo(inner) + Right.ApplyTo(inner);

        public override Function WithVariable(Symbol newVariable) => Left.WithVariable(newVariable) + Right.WithVariable(newVariable);

        public override bool Equals(Function? other) => other is Sum s && s.Left == Left && s.Right == Right;

        public override string ToString(string inner) => $"({Left.ToString(inner)}) + ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) + ({Right})";

        public override string ToPrefixString(string inner) => $"+ {Left.ToPrefixString(inner)} {Right.ToPrefixString(inner)}";

        public override string ToPrefixString() => $"+ {Left.ToPrefixString()} {Right.ToPrefixString()}";

        protected override Function _diff(Symbol variable) => Left.Diff(variable) + Right.Diff(variable);

        protected override Function _integrate(Symbol variable) => Left.Integrate(variable) + Right.Integrate(variable);

        protected override HashCodeCombiner _addHashCodeVariable(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner.Add(Left, Right);
    }
}
