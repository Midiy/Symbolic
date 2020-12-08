using System;

namespace Symbolic.Functions.Standart
{
    public class Log10 : Function
    {
        public override Quotient Diff() => (Quotient)(1 / (new Ln().ApplyTo(10) * Variable!));

        public override double GetValue(double variableValue) => Math.Log10(variableValue);

        public override bool Equals(Function? other) => other is Log10;

        public override string ToString(string? inner) => $"log10({inner})";
    }
}
