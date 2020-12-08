using System;

namespace Symbolic.Functions.Standart
{
    public class Root : Function
    {
        public Constant Degree { get; init; }

        public Root(Symbol variable, Constant degree) : base(variable) => Degree = degree;

        public override Quotient Diff() => (Quotient)(new Power(Variable, (Degree - 1) / Degree) / Degree);

        public override double GetValue(double variableValue) => Math.Pow(variableValue, 1 / Degree);

        public override bool Equals(Function? other) => other is Root r && r.Degree == Degree && other.Variable == Variable;

        public override string ToString(string? inner) => $"({inner})^(1/({Degree}))";
    }
}
