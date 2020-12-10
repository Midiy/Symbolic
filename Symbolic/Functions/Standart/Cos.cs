﻿using System;

namespace Symbolic.Functions.Standart
{
    public class Cos : Function
    {
        public Cos(Symbol variable) : base(variable) => HasAllIntegralsKnown = true;

        public override double GetValue(double variableValue) => Math.Cos(variableValue);

        public override Cos WithVariable(Symbol newVariable) => new Cos(newVariable);

        public override bool Equals(Function? other) => other is Cos && other.Variable == Variable;

        public override string ToString(string inner) => $"cos({inner})";

        protected override Function _diff(Symbol _) => -new Sin(Variable);

        protected override Function _integrate(Symbol _) => new Sin(Variable);
    }
}
