using System;

namespace Symbolic.Functions.Standart
{
    public class Power : Function
    {
        public Constant Exponent { get; init; }

        public Power(Symbol variable, Constant exponent) : base(variable)
        {
            Exponent = exponent;
            HasAllIntegralsKnown = true;
        }

        public override double GetValue(double variableValue) => Math.Pow(variableValue, Exponent);

        public override Function Add(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Power p && Exponent == p.Exponent && Exponent >= 0 && Exponent % 1 == 0) { return new Monomial(Variable, 2, Exponent); }
                else if (other is Monomial m && Exponent == m.Exponent) { return m.Add(this); }
            }
            return base.Add(other);
        }

        public override Function Subtract(Function other)
        {
            if (other is Monomial m && Variable == m.Variable && Exponent == m.Exponent) { return m.Add(this); }
            else { return base.Add(other); }
        }

        public override Function Multiply(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Constant c)
                {
                    if (c == 0) { return 0; }
                    else if (c == 1) { return this; }
                    else if (c == -1) { return this.Negate(); }
                    else { return new Monomial(Variable, c, Exponent); }
                }
                else if (other is Symbol) { return new Power(Variable, Exponent + 1); }
                else if (other is Power pw) { return new Power(Variable, Exponent + pw.Exponent); }
                else if (other is Monomial m) { return m.Multiply(this); }
                else if (other is Polynomial p) { return p.Multiply(this); }
            }
            return base.Multiply(other);
        }

        public override Power WithVariable(Symbol newVariable) => new Power(newVariable, Exponent);

        public override bool Equals(Function? other) => other is Power p && p.Exponent == Exponent && other.Variable == Variable;

        public override string ToString(string inner) => $"({inner})^({Exponent})";

        protected override Function _diff(Symbol _) => Exponent * (Variable ^ (Exponent - 1));

        protected override Function _integrate(Symbol _) => (Variable ^ (Exponent + 1)) / (Exponent + 1);

        protected override int _getHashCodePart2() => unchecked(47 * Exponent.GetHashCode());
    }
}
