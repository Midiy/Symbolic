using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Symbolic.Functions.Standart;
using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    // TODO : Should Polynomial have inner function different from Symbol?
    public class Polynomial : Function
    {
        public IEnumerable<Constant> Coeffs;

        protected Polynomial()
        {
            Coeffs = Array.Empty<Constant>();
            HasAllIntegralsKnown = true;
        }

        public Polynomial(Function inner, IEnumerable<Constant> coeffs, bool reversed = false) : base(inner)
        {
            Coeffs = (reversed ? coeffs : coeffs.Reverse());
            if (Coeffs.Count() == 0) { Coeffs = new Constant[] { 0 }; }
            HasAllIntegralsKnown = true;
            PriorityWhenInner = Priorities.Addition;
            PriorityWhenOuter = Priorities.Exponentiation;
        }

        public Polynomial(Function inner, params Constant[] coeffs) : this(inner, coeffs, false) { }

        // TODO : Add Inner.
        public Polynomial(IEnumerable<Monomial> addends)
        {
            // It is hard to validate that all monomials have the same inner function (because of monomials which have zero exponent),
            // so only monomials where inner function is symbol are acceptable.
            if (addends.All((Monomial m) => m.Inner is Symbol)) { throw new Exception(); }   // TODO : Specify exception type.
            Symbol variable = addends.Aggregate(Symbol.ANY, (Symbol acc, Monomial curr) => acc | curr.Variable);
            if (!addends.All((Monomial m) => m.Variable == variable)) { throw new Exception(); }   // TODO : Specify exception type.
            addends = addends.OrderBy((Monomial m) => m.Exponent);
            int maxPow = (int)addends.Last().Exponent;
            Constant[] coeffs = new Constant[maxPow + 1];
            int position = 0;
            foreach (Monomial m in addends)
            {
                while (m.Exponent > position) { coeffs[position++] = 0; }
                coeffs[position++] = m.Coefficient;
            }
            Coeffs = coeffs;
            Inner = variable;
            Variable = variable;
            HasAllIntegralsKnown = true;
        }

        public Polynomial(params Monomial[] addends) : this((IEnumerable<Monomial>)addends) { }

        public override string ToPrefixString()
        {
            // Constructor is used because this monomials are temporary objects that not expected to be cached.
            IEnumerable<string> monomialReprs = Coeffs.Select((Constant coeff, int num) => new Monomial(Inner, coeff, num).ToPrefixString()).Where((string s) => s != "( 0 )");
            if (!monomialReprs.Any()) { return "( 0 )"; }
            else if (monomialReprs.Count() == 1) { return monomialReprs.Single(); }
            else { return string.Join(' ', Enumerable.Repeat('+', monomialReprs.Count() - 1)) + " " + string.Join(' ', monomialReprs); }
        }

        protected override double _getValue(double variableValue) => Coeffs.Select((Constant coeff, int num) => coeff * Math.Pow(variableValue, num)).Aggregate((Constant acc, Constant curr) => acc + curr);

        protected override Function _applyTo(Function inner) => Polynomial(inner, Coeffs, true);

        protected override bool _equals(Function? other) => other is Polynomial p && Coeffs.SequenceEqual(p.Coeffs);

        protected override string _toString()
        {
            // Constructor is used because this monomials are temporary objects that not expected to be cached.
            IEnumerable<string> monomialStrings = Coeffs.Select((Constant coeff, int num) => new Monomial(Inner, coeff, num).ToString(Priorities.Addition))
                                                        .Where((string s) => s != "0")
                                                        .Reverse();
            if (!monomialStrings.Any()) { return "0"; }
            else
            {
                StringBuilder builder = new StringBuilder(monomialStrings.First().Trim('(', ')'));
                foreach (string s in monomialStrings.Skip(1))
                {
                    if (s.StartsWith("(-")) 
                    { 
                        builder.Append(" - ").Append(s.Substring(2).TrimStart().TrimEnd(')').TrimEnd());
                    }
                    else { builder.Append(" + ").Append(s); }
                }
                return builder.ToString();
            }
        }

        protected override Function _diff(Symbol _)
        {
            if (Coeffs.Count() == 1) { return 0; }
            else if (Coeffs.Count() == 2) { return Coeffs.Last(); }
            else { return Polynomial(Inner, Coeffs.Skip(1).Select((Constant coeff, int num) => coeff * (num + 1)), true); }
        }

        protected override Function _integrate(Symbol variable)
        {
            if (Coeffs.Count() == 1)
            {
                if (Coeffs.First() == 0) { return 0; }
                else { return Monomial(variable, Coeffs.First(), 1); }
            }
            else { return Polynomial(Variable, Coeffs.Select((Constant coeff, int num) => coeff / (num + 1)).Prepend(0), true); }
        }

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.AddEnumerable(Coeffs);

        public static explicit operator Polynomial(Power power)
        {
            if (power.Exponent < 0 || power.Exponent % 1 != 0) { throw new InvalidCastException(); }
            return Polynomial(power.Variable, Enumerable.Repeat(Constant.Zero, (int)power.Exponent).Prepend(1));
        }
    }
}
