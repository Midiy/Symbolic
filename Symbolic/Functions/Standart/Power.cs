using System;

namespace Symbolic.Functions.Standart
{
    public class Power : Function
    {
        public Constant Exponent { get; init; }

        public Power(Symbol variable, Constant exponent) : base(variable) => Exponent = exponent;

        public override double GetValue(double variableValue) => Math.Pow(variableValue, Exponent);

        public override Power WithVariable(Symbol newVariable) => new Power(newVariable, Exponent);

        public override bool Equals(Function? other) => other is Power p && p.Exponent == Exponent && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"({inner})^({Exponent})";

        protected override Function _diff(Symbol variable) => Exponent * new Power(Variable!, Exponent - 1);
    }
}
