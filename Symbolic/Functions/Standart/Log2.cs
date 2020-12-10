using System;

namespace Symbolic.Functions.Standart
{
    public class Log2 : Log
    {
        public Log2(Symbol variable) : base(variable, 2) { }

        public override double GetValue(double variableValue) => Math.Log2(variableValue);

        public override Log2 WithVariable(Symbol newVariable) => new Log2(newVariable);

        public override string ToString(string? inner) => $"log2({inner})";
    }
}
