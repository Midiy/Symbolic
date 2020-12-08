using System;

namespace Symbolic.Functions.Standart
{
    class Cos : Function
    {
        public override Negation Diff() => (Negation)(-new Sin());

        public override double GetValue(double variableValue) => Math.Cos(variableValue);

        public override bool Equals(Function? other) => other is Cos;

        public override string ToString(string? inner) => $"cos({inner})";
    }
}
