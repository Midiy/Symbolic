using System;
using System.Collections.Generic;
using System.Linq;

using Symbolic.Functions.Standart;

namespace Symbolic.Functions
{
    public class Polynomial : Function
    {
        public IEnumerable<Constant> Coeffs;

        protected Polynomial() { }

        public Polynomial(Symbol variable, IEnumerable<Constant> coeffs, bool reversed = false) : base(variable)
        {
            Coeffs = (reversed ? coeffs : coeffs.Reverse());
            if (Coeffs.Count() == 0) { Coeffs = new Constant[] { 0 }; }
            HasAllIntegralsKnown = true;
        }

        public Polynomial(Symbol variable, params Constant[] coeffs) : this(variable, coeffs, false) { }

        public Polynomial(IEnumerable<Monomial> addends)
        {
            Symbol variable = addends.FirstOrDefault((Monomial m) => m.Variable != Symbol.ANY)?.Variable ?? Symbol.ANY;
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
            Variable = variable;
            HasAllIntegralsKnown = true;
        }

        public Polynomial(params Monomial[] addends) : this((IEnumerable<Monomial>)addends) { }

        public override double GetValue(double variableValue) => Coeffs.Select((Constant coeff, int num) => coeff * Math.Pow(variableValue, num)).Aggregate((Constant acc, Constant curr) => acc + curr);

        public override Polynomial Negate() => new Polynomial(Variable, Coeffs.Select((Constant c) => -c), true);

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
                    return new Polynomial(Variable, newCoeffs, true);
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
                    return new Polynomial(Variable, newCoeffs, true);
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
                    Constant[] newCoeffs = Enumerable.Repeat(new Constant(0), leftLen + rightLen - 1).ToArray();
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
                    return new Polynomial(Variable, newCoeffs, true);
                }
                else if (other is Power pw && pw.Exponent >= 0 && pw.Exponent % 1 == 0) { return this.Multiply((Polynomial)pw); }
            }
            return base.Multiply(other);
        }

        public override Function Divide(Function other)
        {
            if (other is Constant c && c != 0) { return new Polynomial(Variable, Coeffs.Select((Constant coeff) => coeff / c), true); }
            return base.Divide(other);
        }

        public override Function Raise(Function other)
        {
            if (other is Constant c && c % 1 == 0)
            {
                Polynomial result = new Polynomial(Variable, new Constant[] { 1 });
                for (int i = 0; i < c; i++) { result = (Polynomial)(result * this); }
                return result;
            }
            else { return base.Raise(other); }
        }

        public override Function WithVariable(Symbol newVariable) => new Polynomial(newVariable, Coeffs, true);

        public override bool Equals(Function? other) => other is Polynomial p && Coeffs.SequenceEqual(p.Coeffs);

        public override string ToString(string? inner)
        {
            // TODO : Rewrite from scratch.
            string str = Coeffs.Select((Constant coeff, int num) =>
            {
                string result = "";
                if (coeff != 0)
                {
                    if (num != 0)
                    {
                        string strCoeff = "";
                        if (coeff == -1) { strCoeff = "-"; }
                        else if (coeff != 1) { strCoeff = coeff.ToString(); }

                        if (num != 1) { result += $"{strCoeff}({inner})^{num}"; }
                        else { result += $"{strCoeff}({inner})"; }
                    }
                    else { result += coeff.ToString(); }
                }
                return result;
            })
                              .Reverse()
                              .Aggregate((string acc, string curr) =>
                              {
                                  bool signFlag = false;
                                  if (curr.StartsWith("-"))
                                  {
                                      curr = curr[1..];
                                      signFlag = true;
                                  }
                                  return curr == "" ? acc : $"{acc} {(signFlag ? "-" : (acc == "" ? "" : "+"))} {curr}";
                              }).Trim();
            if (str == "") { str = "0"; }
            return str;
        }

        protected override Function _diff(Symbol _)
        {
            if (Coeffs.Count() == 1) { return 0; }
            else if (Coeffs.Count() == 2) { return Coeffs.Last(); }
            else { return new Polynomial(Variable, Coeffs.Skip(1).Select((Constant coeff, int num) => coeff * (num + 1)), true); }
        }

        protected override Function _integrate(Symbol variable)
        {
            if (Coeffs.Count() == 1)
            {
                if (Coeffs.First() == 0) { return 0; }
                else { return new Monomial(variable, Coeffs.First(), 1); }
            }
            else { return new Polynomial(Variable, Coeffs.Select((Constant coeff, int num) => coeff / (num + 1)).Prepend(0), true); }
        }

        public static explicit operator Polynomial(Power power)
        {
            if (power.Exponent < 0 && power.Exponent % 1 != 0) { throw new InvalidCastException(); }
            return new Polynomial(power.Variable, Enumerable.Repeat(new Constant(0), (int)power.Exponent).Prepend(1));
        }
    }
}
