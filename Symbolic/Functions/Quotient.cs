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
        }

        public override Function Diff() => (Left.Diff() * Right - Left * Right.Diff()) / (Right ^ 2);

        public override double GetValue(double variableValue) => Left.GetValue(variableValue) / Right.GetValue(variableValue);

        public override Function Multiply(Function other) => other == Right ? Left : base.Multiply(other);

        public override Function Divide(Function other) => other is Quotient q ? (Left * q.Right) / (Right * q.Left) : Left / (Right * other);

        public override Function ApplyTo(Function inner) => Left.ApplyTo(inner) / Right.ApplyTo(inner);

        public override bool Equals(Function? other) => other is Quotient q && q.Left == Left && q.Right == Right;

        public override string ToString(string? inner) => $"({Left.ToString(inner)}) / ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) / ({Right})";
    }
}
