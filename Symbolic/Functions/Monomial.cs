using System;
using System.Linq;

using Symbolic.Functions.Standart;
using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public class Monomial : Polynomial
    {
        public Constant Coefficient { get; init; }
        public Constant Exponent { get; init; }

        protected Monomial() : base() { }

        public Monomial(Symbol variable, Constant coefficient, Constant exponent)
        {
            (Coefficient, Exponent) = (coefficient, exponent);
            Variable = Exponent == 0 ? Symbol.ANY : variable;
            Coeffs = Enumerable.Repeat(Constant.Zero, (int)Exponent).Append(Coefficient);
        }

        public override double GetValue(double variableValue) => Coefficient * Math.Pow(variableValue, Exponent);

        public override Monomial Negate() => Monomial(Variable, -Coefficient, Exponent);

        public override Function Add(Function other)
        {
            if (other != 0 && Variable == other.Variable && other is Monomial m)
            {
                if (Exponent == m.Exponent) { return Monomial(Variable | m.Variable, Coefficient + m.Coefficient, Exponent); }
                else { return Polynomial(this, m); }
            }
            else { return base.Add(other); }
        }

        public override Function Subtract(Function other)
        {
            if (other != 0 && Variable == other.Variable && other is Monomial m)
            {
                if (Exponent == m.Exponent) { return Monomial(Variable | m.Variable, Coefficient - m.Coefficient, Exponent); }
                else { return Polynomial(this, m.Negate()); }
            }
            else { return base.Subtract(other); }
        }

        public override Function Multiply(Function other)
        {
            if (other != 0 && Variable == other.Variable && other is Monomial m) { return Monomial(Variable | m.Variable, Coefficient * m.Coefficient, Exponent + m.Exponent); }
            return base.Multiply(other);
        }

        public override Function Divide(Function other)
        {
            if (other is Constant c) 
            {
                if (c == 0) { throw new DivideByZeroException(); }
                else if (c == 1) { return this; }
                else { return Monomial(Variable, Coefficient / c, Exponent); }
            }
            else { return base.Divide(other); }
        }

        public override Function Raise(Function other)
        {
            if (other is Constant c && c % 1 == 0 && c > 0) { return Monomial(Variable, Coefficient, Exponent * c); }
            else { return base.Raise(other); }
        }

        public override Function ApplyTo(Function inner) => Coefficient * (inner ^ Exponent);

        public override Function WithVariable(Symbol newVariable) => Monomial(newVariable, Coefficient, Exponent);

        public override bool Equals(Function? other) => other is Monomial m && Coefficient == m.Coefficient && Exponent == m.Exponent;

        public override string ToString(string inner) => $"{Coefficient}*({inner})^{Exponent}";

        public override string ToPrefixString(string inner)
        {
            if (Coefficient == 0) { return "( 0 )"; }
            if (Exponent == 0) { return Coefficient.ToPrefixString(); }
            string coeffStr = Coefficient == 1 ? "" : $"* {Coefficient.ToPrefixString()} ";
            if (Exponent == 1) { return coeffStr + inner; }
            // Generally $"* {Coefficient.ToPrefixString()} ^ {inner} {Exponent.ToPrefixString()}".
            else { return coeffStr + $"^ {inner} {Exponent.ToPrefixString()}"; }
        }

        protected override Function _diff(Symbol variable)
        {
            if (Coefficient == 0 || Exponent == 0) { return 0; }
            else if (Exponent == 1) { return Coefficient; }
            else { return Monomial(variable, Coefficient * Exponent, Exponent - 1); }
        }

        protected override Function _integrate(Symbol variable)
        {
            if (Coefficient == 0) { return 0; }
            else if (Exponent == 0) { return Coefficient * variable; }
            else { return Monomial(Variable, Coefficient / (Exponent + 1), Exponent + 1); }
        }

        protected override HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner.Add(Coefficient).Add(Exponent);

        public static explicit operator Monomial(Power power)
        {
            if (power.Exponent < 0 && power.Exponent % 1 != 0) { throw new InvalidCastException(); }
            return Monomial(power.Variable, 1, power.Exponent);
        }
    }
}
