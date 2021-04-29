using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Log10 : Log
    {
        public Log10(Symbol variable) : base(variable, 10) { }

        public override double GetValue(double variableValue) => Math.Log10(variableValue);

        public override Log10 WithVariable(Symbol newVariable) => Log10(newVariable);

        public override string ToString(string inner) => $"log10({inner})";

        public override string ToPrefixString(string inner) => $"log10 {inner}";
    }
}
