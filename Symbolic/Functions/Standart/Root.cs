using System;

namespace Symbolic.Functions.Standart
{
    public class Root : Function
    {
        public Constant Degree { get; init; }

        public Root(Constant degree) => Degree = degree;

        public override Quotient Diff() => (Quotient)(new Power((Degree - 1) / Degree) / Degree);

        public override double GetValue(double variableValue) => Math.Pow(variableValue, 1 / Degree);

        public override bool Equals(Function? other) => other is Root r && r.Degree == Degree;

        public override string ToString(string? inner) => $"({inner})^(1/({Degree}))";
    }
}
