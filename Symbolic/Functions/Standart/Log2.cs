using System;

namespace Symbolic.Functions.Standart
{
    public class Log2 : Function
    {
        public Log2(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => Math.Log2(variableValue);

        public override Log2 WithVariable(Symbol newVariable) => new Log2(newVariable);

        public override bool Equals(Function? other) => other is Log2 && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"log2({inner})";

        protected override Function _diff(Symbol variable) => 1 / (new Ln(Variable!).ApplyTo(2) * Variable!);
    }
}
