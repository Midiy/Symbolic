using System;

namespace Symbolic.Functions.Standart
{
    class Ln : Function
    {
        public override Quotient Diff() => (Quotient)(1 / Variable!);

        public override double GetValue(double variableValue) => Math.Log(variableValue);

        public override bool Equals(Function? other) => other is Ln;

        public override string ToString(string? inner) => $"ln({inner})";
    }
}
