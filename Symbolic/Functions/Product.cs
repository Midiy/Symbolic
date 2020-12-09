namespace Symbolic.Functions
{
    public class Product : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Product(Function left, Function right)
        {
            (Left, Right) = (left, right);
            if (Left.Variable == Right.Variable) { Variable = Left.Variable; }
        }

        public override Function Diff(Symbol variable) => Left.Diff(variable) * Right + Left * Right.Diff(variable);

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

        public override Function WithVariable(Symbol newVariable) => Left.WithVariable(newVariable) * Right.WithVariable(newVariable);

        public override bool Equals(Function? other) => other is Product p && (p.Left == Left && p.Right == Right || p.Left == Right && p.Right == p.Left);

        public override string ToString(string? inner) => $"({Left.ToString(inner)}) * ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) * ({Right})";

        protected override Function _diff(Symbol variable) => throw new System.NotImplementedException();
    }
}
