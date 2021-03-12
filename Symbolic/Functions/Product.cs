using Symbolic.Functions.Standart;

namespace Symbolic.Functions
{
    public class Product : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        public Product(Function left, Function right)
        {
            (Left, Right) = (left, right);
            if (Left.Variable == Right.Variable) { Variable = Left.Variable | Right.Variable; }
            HasAllIntegralsKnown = (Left.HasAllIntegralsKnown && (Right is Polynomial || Right is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0)) ||
                                   (Right.HasAllIntegralsKnown && (Left is Polynomial || Left is Power pw1 && pw1.Exponent >= 0 && pw1.Exponent % 1 == 0));
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

        public override string ToString(string inner) => $"({Left.ToString(inner)}) * ({Right.ToString(inner)})";

        public override string ToString() => $"({Left}) * ({Right})";

        protected override Function _diff(Symbol variable) => Left.Diff(variable) * Right + Right.Diff(variable) * Left;

        protected override Function _integrate(Symbol variable)
        {
            if (HasAllIntegralsKnown)
            {
                Function u, dv;
                if (Left is Polynomial || Left is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0)
                {
                    u = Left;
                    dv = Right;
                }
                else
                {
                    u = Right;
                    dv = Left;
                }
                Function result = 0;
                bool isEvenIter = true;
                do
                {
                    dv = dv.Integrate(variable);
                    result += (isEvenIter ? 1 : -1) * u * dv;
                    u = u.Diff(variable);
                    isEvenIter = !isEvenIter;
                } while (u != 0);
                return result;
            }
            else { throw new System.NotImplementedException(); }
        }
    }
}
