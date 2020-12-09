namespace Symbolic.Functions
{
    public class Sum : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Sum(Function left, Function right)
        {
            (Left, Right) = (left, right);
            if (Left.Variable == Right.Variable) { Variable = Left.Variable; }
        }

        public override Function Diff(Symbol variable) => Left.Diff(variable) + Right.Diff(variable);

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

        public override string ToString(string? inner) => $"({Left.ToString(inner)}) + ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) + ({Right})";

        protected override Function _diff(Symbol variable) => throw new System.NotImplementedException();
    }
}
