using System;

namespace Symbolic.Functions.Standart
{
    public class Root : Function
    {
        public Constant Degree { get; init; }

        public Root(Symbol variable, Constant degree) : base(variable) => Degree = degree;

        public override double GetValue(double variableValue) => Math.Pow(variableValue, 1 / Degree);

        public override Root WithVariable(Symbol newVariable) => new Root(newVariable, Degree);

        public override bool Equals(Function? other) => other is Root r && r.Degree == Degree && other.Variable == Variable;

        public override string ToString(string? inner) => $"({inner})^(1/({Degree}))";

        protected override Function _diff(Symbol variable) => new Power(Variable, (Degree - 1) / Degree) / Degree;
    }
}
