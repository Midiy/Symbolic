﻿using System;

namespace Symbolic.Functions.Standart
{
    public class Sqrt : Function
    {
        public Sqrt(Symbol variable) : base(variable) { }

        public override Quotient Diff() => (Quotient)(1 / (2 * new Sqrt(Variable!)));

        public override double GetValue(double variableValue) => Math.Sqrt(variableValue);

        public override bool Equals(Function? other) => other is Sqrt && other.Variable! == Variable!;

        public override string ToString(string? inner) => $"sqrt({inner})";
    }
}