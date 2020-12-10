using System;

namespace Symbolic.Functions.Standart
{
    public class Root : Power
    {
        public Constant Degree { get; init; }

        public Root(Symbol variable, Constant degree) : base(variable, 1 / degree) => Degree = degree;

        public override Root WithVariable(Symbol newVariable) => new Root(newVariable, Degree);
    }
}
