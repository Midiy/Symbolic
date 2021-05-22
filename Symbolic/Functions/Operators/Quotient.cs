using Symbolic.Utils;

namespace Symbolic.Functions.Operators
{
    public class Quotient : Operator
    {
        public Quotient(Function left, Function right) : base(left, right, false, "/")
        {
            if (right is Constant c && c.Value == 0) { throw new System.DivideByZeroException(); }
            if (Right is Constant) { HasAllIntegralsKnown = Left.HasAllIntegralsKnown; }
            PriorityWhenInner = Priorities.Division;
            PriorityWhenOuter = Priorities.Division;
        }

        protected override double _getValue(double leftValue, double rightValue) => leftValue / rightValue;

        protected override Function _applyTo(Function left, Function right) => left / right;
        
        protected override Function _diff(Symbol variable) => (Left.Diff(variable) * Right - Right.Diff(variable) * Left) / (Right ^ 2);

        protected override Function _integrate(Symbol variable)
        {
            if (Right is Constant c) { return Left.Integrate(variable) / c; }
            else { throw new System.NotImplementedException(); }
        }
    }
}
