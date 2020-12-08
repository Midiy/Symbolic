namespace Symbolic.Functions
{
    public class Sum : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Sum(Function left, Function right) => (Left, Right) = (left, right);

        public override Function Diff() => Left.Diff() + Right.Diff();

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

        public override bool Equals(Function? other) => other is Sum s && s.Left == Left && s.Right == Right;

        public override string ToString(string? inner) => $"({Left.ToString(inner)}) + ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) + ({Right})";
    }
}
