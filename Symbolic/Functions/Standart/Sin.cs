using System;

namespace Symbolic.Functions.Standart
{
    class Sin : Function
    {
        public override Cos Diff() => new Cos();

        public override double GetValue(double variableValue) => Math.Sin(variableValue);

        public override bool Equals(Function? other) => other is Sin;

        public override string ToString(string? inner) => $"sin({inner})";
    }
}
