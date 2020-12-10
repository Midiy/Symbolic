using System;

using Symbolic.Functions.Standart;

namespace Symbolic.Functions
{
    public class Symbol : Function
    {
        public static readonly Symbol ANY = new Symbol("ANY") { _isAny = true };

        public string StrSymbol { get; init; }

        private bool _isAny { get; init; } = false;

        public Symbol(string strSymbol)
        {
            StrSymbol = strSymbol;
            Variable = this;
        }

        public override double GetValue(double variableValue) => variableValue;

        public override Function Negate() => new Monomial(this, -1, 1);

        public override Function Add(Function other)
        {
            if (this == other.Variable)
            {
                if (other is Constant c)
                {
                    if (other == 0) { return this; }
                    else { return new Polynomial(this, 1, c); }
                }
                else if (other == this) { return new Monomial(this, 2, 1); }
                else if (other is Power pw && this == pw.Variable && pw.Exponent % 1 == 0) { return ((Monomial)pw).Add(this); }
                else if (other is Monomial || other is Polynomial) { return other.Add(this); }
            }
            return base.Add(other);
        }

        public override Function Subtract(Function other)
        {
            if (this == other.Variable)
            {
                if (other is Constant c)
                {
                    if (other == 0) { return this; }
                    else { return new Polynomial(this, 1, -c); }
                }
                else if (other is Power pw && this == pw.Variable && pw.Exponent % 1 == 0) { return ((Monomial)pw).Negate().Add(this); }
                else if (other is Monomial m) { return m.Negate().Add(this); }
                else if (other is Polynomial p) { return p.Negate().Add(this); }
            }
            return base.Subtract(other);
        }

        public override Function Multiply(Function other)
        {
            if (this == other.Variable)
            {
                if (other is Constant c)
                {
                    if (other == 0) { return 0; }
                    else if (other == 1) { return this; }
                    else { return new Monomial(this, c, 1); }
                }
                else if (other == this) { return new Monomial(this, 1, 2); }
                else if (other is Power pw && this == pw.Variable && pw.Exponent % 1 == 0) { return ((Monomial)pw).Multiply(this); }
                else if (other is Monomial m) { return m.Multiply(this); }
                else if (other is Polynomial p) { return p.Multiply(this); }
            }
            return base.Multiply(other);
        }

        public override Function Divide(Function other)
        {
            if (other is Constant c)
            {
                if (c == 0) { throw new DivideByZeroException(); }
                else if (c == 1) { return this; }
                else { return new Monomial(this, 1 / c, 1); }
            }
            return base.Divide(other);
        }

        public override Function Raise(Function other)
        {
            if (other is Constant c)
            {
                if (c % 1 == 0 && c > 0) { return new Monomial(this, 1, c); }
                else { return new Power(this, c); }
            }
            else { return base.Raise(other); } 
        }

        public override Function ApplyTo(Function inner) => inner;

        public override Symbol WithVariable(Symbol newVariable) => newVariable;

        public override bool Equals(Function? other) => other is Symbol s && (s.StrSymbol == StrSymbol || _isAny || s._isAny);

        public override string ToString(string? inner) => inner;

        public override string ToString() => StrSymbol;

        protected override Constant _diff(Symbol _) => 1;
    }
}
