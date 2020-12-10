using System;

using Symbolic.Functions.Standart;

namespace Symbolic.Functions
{
    public class Monomial : Function
    {
        public Constant Coefficient { get; init; }
        public Constant Exponent { get; init; }

        public Monomial(Symbol variable, Constant coefficient, Constant exponent) : base(exponent == 0 ? Symbol.ANY : variable)
        {
            (Coefficient, Exponent) = (coefficient, exponent);
            HasAllIntegralsKnown = true;
        }

        public override double GetValue(double variableValue) => Coefficient * Math.Pow(variableValue, Exponent);

        public override Monomial Negate() => new Monomial(Variable, -Coefficient, Exponent);

        public override Function Add(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Monomial m)
                {
                    if (Exponent == m.Exponent) { return new Monomial(Variable, Coefficient + m.Coefficient, Exponent); }
                    else { return new Polynomial(this, m); }
                }
                else if (other is Polynomial p) { return p.Add(this); }
                else if (other is Constant c && other != 0) { return this.Add((Monomial)c); }
                else if (other is Symbol s) { return this.Add((Monomial)s); }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Add((Monomial)pw); }
            }
            return base.Add(other);
        }

        public override Function Subtract(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Monomial m)
                {
                    if (Exponent == m.Exponent) { return new Monomial(Variable, Coefficient - m.Coefficient, Exponent); }
                    else { return new Polynomial(this, m.Negate()); }
                }
                else if (other is Polynomial p) { return p.Subtract(this); }
                else if (other is Constant c && other != 0) { return this.Subtract((Monomial)c); }
                else if (other is Symbol s) { return this.Subtract((Monomial)s); }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Subtract((Monomial)pw); }
            }
            return base.Subtract(other);
        }

        public override Function Multiply(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Monomial m) { return new Monomial(Variable, Coefficient * m.Coefficient, Exponent + m.Exponent); }
                else if (other is Polynomial p) { return p.Multiply(this); }
                else if (other is Constant c) 
                {
                    if (other == 0) { return 0; }
                    else if (other == 1) { return this; }
                    else if (other == -1) { return this.Negate(); }
                    else { return this.Multiply((Monomial)c); }
                }
                else if (other is Symbol s) { return this.Multiply((Monomial)s); }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Multiply((Monomial)pw); }
            }
            return base.Multiply(other);
        }

        public override Function Divide(Function other)
        {
            if (other is Constant c) 
            {
                if (c == 0) { throw new DivideByZeroException(); }
                else if (c == 1) { return this; }
                else { return new Monomial(Variable, Coefficient / c, Exponent); }
            }
            else { return base.Divide(other); }
        }

        public override Function Raise(Function other)
        {
            if (other is Constant c && c % 1 == 0 && c > 0) { return new Monomial(Variable, Coefficient, Exponent * c); }
            else { return base.Raise(other); }
        }

        public override Function ApplyTo(Function inner) => Coefficient * (inner ^ Exponent);

        public override Function WithVariable(Symbol newVariable) => new Monomial(newVariable, Coefficient, Exponent);

        public override bool Equals(Function? other) => other is Monomial m && Coefficient == m.Coefficient && Exponent == m.Exponent;

        public override string ToString(string inner) => $"{Coefficient}*({inner})^{Exponent}";

        protected override Function _diff(Symbol variable)
        {
            if (Coefficient == 0 || Exponent == 0) { return 0; }
            else if (Exponent == 1) { return Coefficient; }
            else { return new Monomial(variable, Coefficient * Exponent, Exponent - 1); }
        }

        protected override Function _integrate(Symbol variable)
        {
            if (Coefficient == 0) { return 0; }
            else if (Exponent == 0) { return Coefficient * variable; }
            else { return new Monomial(Variable, Coefficient / (Exponent + 1), Exponent + 1); }
        }

        public static explicit operator Monomial(Constant c) => new Monomial(Symbol.ANY, c, 0);

        public static explicit operator Monomial(Symbol variable) => new Monomial(variable, 1, 1);

        public static explicit operator Monomial(Power power)
        {
            if (power.Exponent < 0 && power.Exponent % 1 != 0) { throw new InvalidCastException(); }
            return new Monomial(power.Variable, 1, power.Exponent);
        }
    }
}
