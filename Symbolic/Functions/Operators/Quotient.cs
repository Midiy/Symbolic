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

        protected override double _getValue(double variableValue) => Left.GetValue(variableValue) / Right.GetValue(variableValue);

        protected override Function _applyTo(Function inner) => Left.ApplyTo(inner) / Right.ApplyTo(inner);
        
        protected override Function _diff(Symbol variable) => (Left.Diff(variable) * Right - Right.Diff(variable) * Left) / (Right ^ 2);

        protected override Function _integrate(Symbol variable)
        {
            if (Right is Constant c) { return Left.Integrate(variable) / c; }
            else { throw new System.NotImplementedException(); }
        }
    }
}
