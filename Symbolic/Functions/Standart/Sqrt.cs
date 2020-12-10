﻿using System;

namespace Symbolic.Functions.Standart
{
    public class Sqrt : Root
    {
        public Sqrt(Symbol variable) : base(variable, 2) { }

        public override double GetValue(double variableValue) => Math.Sqrt(variableValue);

        public override Sqrt WithVariable(Symbol newVariable) => new Sqrt(newVariable);

        public override string ToString(string inner) => $"sqrt({inner})";

        protected override Function _diff(Symbol variable) => 1 / (2 * new Sqrt(Variable!));
    }
}
