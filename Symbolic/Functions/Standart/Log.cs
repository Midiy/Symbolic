using System;

namespace Symbolic.Functions.Standart
{
    public class Log : Function
    {
        public Constant Base { get; init; }

        public Log(Symbol variable, Constant @base) : base(variable) => Base = @base;

        public override Quotient Diff() => (Quotient)(1 / (new Ln(Variable!).ApplyTo(Base) * Variable!));

        public override double GetValue(double variableValue) => Math.Log(variableValue) / Math.Log(Base);

        public override Log WithVariable(Symbol newVariable) => new Log(newVariable, Base);

        public override bool Equals(Function? other) => other is Log l && l.Base == Base && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"log({inner}, {Base})";
    }
}
