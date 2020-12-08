using System;

namespace Symbolic.Functions.Standart
{
    class Power : Function
    {
        public Constant Exponent { get; init; }

        public Power(Constant exponent) => Exponent = exponent;

        public override Product Diff() => (Product)(Exponent * new Power(Exponent - 1));

        public override double GetValue(double variableValue) => Math.Pow(variableValue, Exponent);

        public override bool Equals(Function? other) => other is Power p && p.Exponent == Exponent;

        public override string ToString(string? inner) => $"({inner})^({Exponent})";
    }
}
