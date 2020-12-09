using System;

namespace Symbolic.Functions.Standart
{
    public class Log10 : Function
    {
        public Log10(Symbol variable) : base(variable) { }

        public override Quotient Diff() => (Quotient)(1 / (new Ln(Variable!).ApplyTo(10) * Variable!));

        public override double GetValue(double variableValue) => Math.Log10(variableValue);

        public override Log10 WithVariable(Symbol newVariable) => new Log10(newVariable);

        public override bool Equals(Function? other) => other is Log10 && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"log10({inner})";
    }
}
