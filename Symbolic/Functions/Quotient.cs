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
            if (Left.Variable == Right.Variable) { Variable = Left.Variable; }
        }

        public override Function Diff(Symbol variable) => (Left.Diff(variable) * Right - Left * Right.Diff(variable)) / (Right ^ 2);

        public override double GetValue(double variableValue) => Left.GetValue(variableValue) / Right.GetValue(variableValue);

        public override Function Multiply(Function other) => other == Right ? Left : base.Multiply(other);

        public override Function Divide(Function other) => other is Quotient q ? (Left * q.Right) / (Right * q.Left) : Left / (Right * other);

        public override Function ApplyTo(Function inner) => Left.ApplyTo(inner) / Right.ApplyTo(inner);

        public override Function WithVariable(Symbol newVariable) => Left.WithVariable(newVariable) / Right.WithVariable(newVariable);

        public override bool Equals(Function? other) => other is Quotient q && q.Left == Left && q.Right == Right;

        public override string ToString(string inner) => $"({Left.ToString(inner)}) / ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) / ({Right})";

        protected override Function _diff(Symbol variable) => throw new System.NotImplementedException();
    }
}
