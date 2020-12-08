namespace Symbolic.Functions
{
    public class Product : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Product(Function left, Function right) => (Left, Right) = (left, right);

        public override Function Diff() => Left.Diff() * Right + Left * Right.Diff();

        public override double GetValue(double variableValue) => Left.GetValue(variableValue) * Right.GetValue(variableValue);

        public override Function Divide(Function other)
        {
            if (other == Left) { return Right; }
            else if (other == Right) { return Left; }
            else if (other is Product p)
            {
                if (p.Left == Left) { return Right.Divide(p.Right); }
                else if (p.Left == Right) { return Left.Divide(p.Right); }
                else if (p.Right == Left) { return Right.Divide(p.Left); }
                else if (p.Right == Right) { return Left.Divide(Left); }
            }
            return base.Divide(other);
        }

        public override Function ApplyTo(Function inner) => Left.ApplyTo(inner) * Right.ApplyTo(inner);

        public override bool Equals(Function? other) => other is Product p && (p.Left == Left && p.Right == Right || p.Left == Right && p.Right == p.Left);

        public override string ToString(string? inner) => $"({Left.ToString(inner)}) * ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) * ({Right})";
    }
}
