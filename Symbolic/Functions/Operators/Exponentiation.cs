using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Operators
{
    public class Exponentiation : Operator
    {
        public Function Base { get => Left; }
        public Function Exponent { get => Right; }

        public Exponentiation(Function @base, Function exponent) : base(@base, exponent, false, "^")
        {
            PriorityWhenInner = Priorities.Exponentiation;
            PriorityWhenOuter = Priorities.Exponentiation;
        }

        public override Function Diff(Symbol variable) => this * (Exponent.Diff(variable) * Ln(Base) + Exponent * Base.Diff(variable) / Base);

        protected override double _getValue(double variableValue) => Math.Pow(Base.GetValue(variableValue), Exponent.GetValue(variableValue));

        protected override Function _applyTo(Function inner) => Base.ApplyTo(inner) ^ Exponent.ApplyTo(inner);

        protected override Function _diff(Symbol variable) => Diff(variable);

        protected override Function _integrate(Symbol _) => throw new NotImplementedException();
    }
}
