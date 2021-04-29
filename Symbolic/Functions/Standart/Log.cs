using System;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Log : Function
    {
        public Constant Base { get; init; }

        public Log(Symbol variable, Constant @base) : base(variable)
        {
            Base = @base;
            HasAllIntegralsKnown = true;
        }

        public override double GetValue(double variableValue) => Math.Log(variableValue) / Math.Log(Base);

        public override Log WithVariable(Symbol newVariable) => Log(newVariable, Base);

        public override bool Equals(Function? other) => other is Log l && l.Base == Base && other.Variable == Variable;

        public override string ToString(string inner) => $"log({inner}, {Base})";

        public override string ToPrefixString(string inner) => $"log {inner} {Base.ToPrefixString()}";

        protected override Function _diff(Symbol _) => 1 / (Ln(Base) * Variable);

        protected override Function _integrate(Symbol _) => (Variable * Ln(Variable) - Variable) / Ln(Base);

        protected override HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner.Add(Base);
    }
}
