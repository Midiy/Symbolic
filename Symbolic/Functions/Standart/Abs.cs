using System;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Abs : Partial
    {
        public Abs(Symbol variable) : base(new (Function, Constant, Constant)[] {
                                                                                    (-variable, Constant.NegativeInfinity, 0),
                                                                                    (variable, 0, Constant.PositiveInfinity)
                                                                                }) { }
            
        public override double GetValue(double variableValue) => Math.Abs(variableValue);

        public override Function Raise(Function other) => (other is Constant c && c % 2 == 0) ? Power(Variable, c) : base.Raise(other);

        public override Abs WithVariable(Symbol newVariable) => Abs(newVariable);

        public override bool Equals(Function? other) => other is Abs && Variable == other.Variable;

        public override string ToString(string inner) => $"|{inner}|";

        public override string ToPrefixString(string inner) => $"abs {inner}";
    }
}
