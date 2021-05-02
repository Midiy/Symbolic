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

        public override Polynomial Negate() => Polynomial(Variable, Coeffs.Select((Constant c) => -c), true);

        public override Function Add(Function other)
        {
            if (other.Variable == Variable)
            {
                if (other is Polynomial p)
                {
                    int count1 = Coeffs.Count();
                    int count2 = p.Coeffs.Count();
                    IEnumerable<Constant> newCoeffs = Coeffs.Zip(p.Coeffs, (Constant c1, Constant c2) => c1 + c2);
                    if (count1 > count2) { newCoeffs = newCoeffs.Concat(Coeffs.Skip(count2)); }
                    else { newCoeffs = newCoeffs.Concat(p.Coeffs.Skip(count1)); }
                    return Polynomial(Variable | p.Variable, newCoeffs, true);
                }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Add((Polynomial)pw); }
            }
            return base.Add(other);
        }

        public override Function Subtract(Function other)
        {
            if (other.Variable == Variable)
            {
                if (other is Polynomial p)
                {
                    int count1 = Coeffs.Count();
                    int count2 = p.Coeffs.Count();
                    IEnumerable<Constant> newCoeffs = Coeffs.Zip(p.Coeffs, (Constant c1, Constant c2) => c1 - c2);
                    if (count1 > count2) { newCoeffs = newCoeffs.Concat(Coeffs.Skip(count2)); }
                    else { newCoeffs = newCoeffs.Concat(p.Coeffs.Skip(count1).Select((Constant c) => -c)); }
                    return Polynomial(Variable | p.Variable, newCoeffs, true);
                }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Subtract((Polynomial)pw); }
            }
            return base.Subtract(other);
        }

        public override Function Multiply(Function other)
        {
            if (Variable == other.Variable)
            {
                if (other is Polynomial p)
                {
                    int leftLen = Coeffs.Count();
                    int rightLen = p.Coeffs.Count();
                    Constant[] newCoeffs = Enumerable.Repeat(Constant.Zero, leftLen + rightLen - 1).ToArray();
                    int index1 = 0;
                    foreach (double c1 in Coeffs)
                    {
                        int index2 = 0;
                        foreach (double c2 in p.Coeffs)
                        {
                            newCoeffs[index1 + index2] += c1 * c2;
                            index2++;
                        }
                        index1++;
                    }
                    return Polynomial(Variable | p.Variable, newCoeffs, true);
                }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Multiply((Polynomial)pw); }
            }
            return base.Multiply(other);
        }

        public override Function Divide(Function other)
        {
            if (other is Constant c && c != 0) { return Polynomial(Variable, Coeffs.Select((Constant coeff) => coeff / c), true); }
            return base.Divide(other);
        }

        public override Function Raise(Function other)
        {
            if (other is Constant c && c % 1 == 0)
            {
                Polynomial result = Polynomial(Variable, new Constant[] { 1 });
                for (int i = 0; i < c; i++) { result = (Polynomial)(result * this); }
                return result;
            }
            else { return base.Raise(other); }
        }

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
