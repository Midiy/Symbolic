using System;
using System.Linq;

using Symbolic.Functions.Standart;
using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    // TODO : Should Monomial have inner function different from Symbol?
    public class Monomial : Polynomial
    {
        public Constant Coefficient { get; init; }
        public Constant Exponent { get; init; }

        protected Monomial() : base() { }

        public Monomial(Function inner, Constant coefficient, Constant exponent) : base(inner, Enumerable.Repeat(Constant.Zero, (int)exponent).Append(coefficient), true)
        {
            Coefficient = coefficient;
            Exponent = exponent;
            PriorityWhenInner = (coefficient.Value, exponent.Value) switch
                                {
                                    (  _, 0) => Coefficient.PriorityWhenInner,
                                    (< 0, _) => Priorities.InnerNegation,
                                    (  0, _) => Priorities.NonNegativeConstant,
                                    (  1, 1) => Inner.PriorityWhenInner,
                                    (  _, 1) => Priorities.Multiplication,
                                    (  _, _) => Priorities.Exponentiation
                                };
            PriorityWhenOuter = (coefficient.Value, exponent.Value) switch
                                {
                                    (_, 0) or (0, _) => Priorities.Min,   // Invariant.
                                    (_, 1)           => Priorities.Multiplication,
                                    (_, _)           => Priorities.Exponentiation
                                };
        }

        public override string ToPrefixString()
        {
            if (Coefficient == 0) { return "( 0 )"; }
            if (Exponent == 0) { return Coefficient.ToPrefixString(); }
            string coeffStr = Coefficient == 1 ? "" : $"* {Coefficient.ToPrefixString()} ";
            if (Exponent == 1) { return coeffStr + Inner.ToPrefixString(); }
            // Generally $"* {Coefficient.ToPrefixString()} ^ {inner} {Exponent.ToPrefixString()}".
            else { return coeffStr + $"^ {Inner.ToPrefixString()} {Exponent.ToPrefixString()}"; }
        }

        protected override double _getValue(double variableValue) => Coefficient * Math.Pow(variableValue, Exponent);

        protected override Function _applyTo(Function inner) => Coefficient * (inner ^ Exponent);

        protected override bool _equals(Function? other) => other is Monomial m && Coefficient == m.Coefficient && Exponent == m.Exponent;

        protected override string _toString()
        {
            if (Coefficient == 0) { return "0"; }
            if (Exponent == 0) { return Coefficient.ToString(); }
            bool signFlag = Coefficient < 0;
            double coeff = Math.Abs(Coefficient);
            if (coeff == 1 && Exponent == 1) { return $"{(signFlag ? "-" : "")}{Inner.ToString(PriorityWhenOuter)}"; }
            string strWithCoeff = coeff == 1 ? $"{Inner.ToString(PriorityWhenOuter)}" : $"{coeff}*{Inner.ToString(PriorityWhenOuter)}";
            return $"{(signFlag ? "-" : "")}" + strWithCoeff + (Exponent == 1 ? "" : $"^{Exponent.ToString(Priorities.Exponentiation)}");
        }

        protected override Function _diff(Symbol _)
        {
            if (Coefficient == 0 || Exponent == 0) { return 0; }
            else if (Exponent == 1) { return Coefficient; }
            else { return Monomial(Inner, Coefficient * Exponent, Exponent - 1); }
        }

        protected override Function _integrate(Symbol _)
        {
            if (Coefficient == 0) { return 0; }
            else if (Exponent == 0) { return Coefficient * Variable; }
            else { return Monomial(Variable, Coefficient / (Exponent + 1), Exponent + 1); }
        }

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.Add(Coefficient).Add(Exponent);

        public static explicit operator Monomial(Power power)
        {
            if (power.Exponent < 0 && power.Exponent % 1 != 0) { throw new InvalidCastException(); }
            return Monomial(power.Variable, 1, power.Exponent);
        }
    }
}
