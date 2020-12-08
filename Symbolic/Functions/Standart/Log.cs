using System;

namespace Symbolic.Functions.Standart
{
    public class Log : Function
    {
        public Constant Base { get; init; }

        public Log(Constant @base) => Base = @base;

        public override Quotient Diff() => (Quotient)(1 / (new Ln().ApplyTo(Base) * Variable!));

        public override double GetValue(double variableValue) => Math.Log(variableValue) / Math.Log(Base);

        public override bool Equals(Function? other) => other is Log l && l.Base == Base;

        public override string ToString(string? inner) => $"log({inner}, {Base})";
    }
}
