using Symbolic.Utils;

namespace Symbolic.Functions.Operators
{
    public class Sum : Operator
    {
        public Sum(Function left, Function right) : base(left, right, true, "+")
        {
            HasAllIntegralsKnown = Left.HasAllIntegralsKnown && Right.HasAllIntegralsKnown;
            PriorityWhenInner = Priorities.Addition;
            PriorityWhenOuter = Priorities.Addition;
        }

        protected override double _getValue(double variableValue) => Left.GetValue(variableValue) + Right.GetValue(variableValue);

        protected override Function _applyTo(Function inner) => Left.ApplyTo(inner) + Right.ApplyTo(inner);

        protected override Function _diff(Symbol variable) => Left.Diff(variable) + Right.Diff(variable);

        protected override Function _integrate(Symbol variable) => Left.Integrate(variable) + Right.Integrate(variable);

        protected override string _toString()
        {
            if (Right is Negation neg) 
            {
                string leftString = Left.ToString(Left.PriorityWhenInner == PriorityWhenOuter ? Priorities.Min : PriorityWhenOuter);
                string rightString = neg.Inner.ToString(PriorityWhenOuter);
                return $"{leftString} - {rightString}";
            }
            else { return base._toString(); }
        }
    }
}
