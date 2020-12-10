using System;

namespace Symbolic.Functions.Standart
{
    public class Cos : Function
    {
        public Cos(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => Math.Cos(variableValue);

        public override Cos WithVariable(Symbol newVariable) => new Cos(newVariable);

        public override bool Equals(Function? other) => other is Cos && other.Variable! == Variable!;

        public override string ToString(string inner) => $"cos({inner})";

        protected override Function _diff(Symbol variable) => -new Sin(Variable!);
    }
}
