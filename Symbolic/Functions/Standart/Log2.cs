using System;

namespace Symbolic.Functions.Standart
{
    class Log2 : Function
    {
        public override Quotient Diff() => (Quotient)(1 / (new Ln().ApplyTo(2) * Variable!));

        public override double GetValue(double variableValue) => Math.Log2(variableValue);

        public override bool Equals(Function? other) => other is Log2;

        public override string ToString(string? inner) => $"log2({inner})";
    }
}
