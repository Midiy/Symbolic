using System;

namespace Symbolic.Functions.Standart
{
    class Exp : Function
    {
        public override Exp Diff() => this;

        public override double GetValue(double variableValue) => Math.Exp(variableValue);

        public override bool Equals(Function? other) => other is Exp;

        public override string ToString(string? inner) => $"exp({inner})";
    }
}
