using System;

using Symbolic.Functions.Standart;
using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public class Symbol : Monomial
    {
        /// <remarks>
        /// Symbol.ANY is expected to be the ONLY instance of Polynomial with Coeffs set to empty array
        /// and the ONLY instance of Monomial with Coefficient and Exponent set to null.
        /// This is caused by a circular dependencies between Symbol and Constant classes.
        /// </remarks>
        public static readonly Symbol ANY = new Symbol();

        public string StrSymbol { get; init; }

        private bool _isAny { get; init; } = false;

        /// <summary>
        /// Constructor only for special value Symbol.ANY.
        /// </summary>
        /// <remarks>
        /// This is a way to work around a circular dependencies between Symbol and Constant classes.
        /// </remarks>
        private Symbol() : base()
        {
            StrSymbol = "ANY";
            Variable = this;
            _isAny = true;
        }

        public Symbol(string strSymbol)
        {
            StrSymbol = strSymbol;
            Variable = this;
            Coefficient = 1;
            Exponent = 1;
            Coeffs = new Constant[] { 0, 1 };
        }

        public override double GetValue(double variableValue) => variableValue;

        public override Monomial Negate() => Monomial(this, -1, 1);

        public override Function Add(Function other)
        {
            if (this == other.Variable)
            {
                if (other is Constant c)
                {
                    if (other == 0) { return this; }
                    else { return Polynomial(this, 1, c); }
                }
                else if (other == this) { return Monomial(this, 2, 1); }
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
                    else { return Polynomial(this, 1, -c); }
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
                    else { return Monomial(this, c, 1); }
                }
                else if (other == this) { return Monomial(this, 1, 2); }
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
                else { return Monomial(this, 1 / c, 1); }
            }
            return base.Divide(other);
        }

        public override Function Raise(Function other)
        {
            if (other is Constant c)
            {
                if (c % 1 == 0 && c > 0) { return Monomial(this, 1, c); }
                else { return Power(this, c); }
            }
            else { return base.Raise(other); }
        }

        public override Function ApplyTo(Function inner) => inner;

        public override Symbol WithVariable(Symbol newVariable) => newVariable;

        public override bool Equals(Function? other) => other is Symbol s && (s.StrSymbol == StrSymbol || _isAny || s._isAny);

        public override string ToString(string inner) => inner;

        public override string ToString() => StrSymbol;

        protected override Constant _diff(Symbol _) => 1;

        protected override Function _integrate(Symbol _) => (this ^ 2) / 2;

        protected override HashCodeCombiner _addHashCodeVariable(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner.Add(StrSymbol);

        public static Symbol operator |(Symbol left, Symbol right) => (left is null || left._isAny) ? right : left;
    }
}
