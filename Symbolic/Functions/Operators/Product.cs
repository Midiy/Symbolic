using Symbolic.Functions.Standart;
using Symbolic.Utils;

namespace Symbolic.Functions.Operators
{
    public class Product : Operator
    {
        public Product(Function left, Function right) : base(left, right, true, "*")
        {
            HasAllIntegralsKnown = (Left.HasAllIntegralsKnown && (Right is Polynomial || Right is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0)) ||
                                   (Right.HasAllIntegralsKnown && (Left is Polynomial || Left is Power pw1 && pw1.Exponent >= 0 && pw1.Exponent % 1 == 0));
            PriorityWhenInner = Priorities.Multiplication;
            PriorityWhenOuter = Priorities.Multiplication;
        }

        public override Function Diff(Symbol variable) => Left.Diff(variable) * Right + Left * Right.Diff(variable);

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

        protected override double _getValue(double variableValue) => Left.GetValue(variableValue) * Right.GetValue(variableValue);

        protected override Function _applyTo(Function inner) => Left.ApplyTo(inner) * Right.ApplyTo(inner);

        protected override Function _diff(Symbol variable) => Diff(variable);

        protected override Function _integrate(Symbol variable)
        {
            if (HasAllIntegralsKnown)
            {
                // Integration by parts.
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
