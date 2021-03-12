using System;

using Symbolic.Functions.Standart;

namespace Symbolic.Functions
{
    public class Exponentiation : Function
    {
        public Function Base { get; init; }
        public Function Exponent { get; init; }

        public Exponentiation(Function @base, Function exponent)
        {
            (Base, Exponent) = (@base, exponent);
            if (Base.Variable == Exponent.Variable) { Variable = Base.Variable | Exponent.Variable; }
        }

        public override Function Diff(Symbol variable) => this * (Exponent.Diff(variable) * new Ln(Variable).ApplyTo(Base) + Exponent * Base.Diff(variable) / Base);

        public override double GetValue(double variableValue) => Math.Pow(Base.GetValue(variableValue), Exponent.GetValue(variableValue));

        public override Function ApplyTo(Function inner) => Base.ApplyTo(inner) ^ Exponent.ApplyTo(inner);

        public override Function WithVariable(Symbol newVariable) => Base.WithVariable(newVariable) ^ Exponent.WithVariable(newVariable);

        public override bool Equals(Function? other) => other is Exponentiation e && e.Base.Equals(Base) && e.Exponent.Equals(Exponent);

        public override string ToString(string inner) => $"({Base.ToString(inner)})^({Exponent.ToString(inner)})";

        public override string ToString() => $"({Base})^({Exponent})";

        protected override Function _diff(Symbol _) => throw new NotImplementedException();

        protected override Function _integrate(Symbol _) => throw new NotImplementedException();
    }
}
