using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Symbolic.Functions;
using Symbolic.Functions.Standart;
using Symbolic.Functions.Operators;

namespace Symbolic.Utils
{
    public static class FunctionFactory
    {
        // Field initialization cannot be used here because of the initialization sequence: the cache may not have been created yet when the field is initialized.
        public static readonly SymbolicConstant E;
        public static readonly SymbolicConstant PI;

        private static ConcurrentDictionary<int, Function> _cache = new();
        private static ConcurrentDictionary<string, Symbol> _symbolCache = new();

        static FunctionFactory() => (E, PI) = (Functions.SymbolicConstant.E, Functions.SymbolicConstant.PI);

        #region Standart functions
        public static Abs Abs(string variable) => Abs(Symbol(variable));

        public static Abs Abs(Symbol variable) => (Abs)Abs((Function)variable);

        public static Function Abs(Function inner)
        {
            if (inner is Negation n) { return n.Inner; }
            else if (inner is Exp || inner is Abs) { return inner; }
            else if (inner is Root r && r.Degree % 2 == 0) { return r; }
            else if (inner is Power pw && (pw.Exponent % 2 == 0 || (1 / pw.Exponent) % 2 == 0)) { return pw; }
            else if (inner is Constant c) { return c >= 0 ? c : Constant(-c.Value); }
            else if (inner is Monomial m && (m.Exponent % 2 == 0)) { return m.Coefficient >= 0 ? m : Monomial(m.Inner, -m.Coefficient, m.Exponent); }
            else if (inner is Quotient q) { return Abs(q.Left) / Abs(q.Right); }
            else if (inner is Product p) { return Abs(p.Left) * Abs(p.Right); }
            else { return _functionFactory(inner, () => new Abs(inner)); }
        }



        public static Cos Cos(string variable) => Cos(Symbol(variable));

        public static Cos Cos(Symbol variable) => (Cos)Cos((Function)variable);

        public static Function Cos(Function inner) => inner is Negation n ? Cos(n.Inner) : _functionFactory(inner, () => new Cos(inner));



        public static Cot Cot(string variable) => Cot(Symbol(variable));

        public static Cot Cot(Symbol variable) => (Cot)Cot((Function)variable);

        public static Function Cot(Function inner) => inner is Negation n ? -Cot(n.Inner) : _functionFactory(inner, () => new Cot(inner));

        public static Cot Ctg(string variable) => Cot(variable);

        public static Cot Ctg(Symbol variable) => Cot(variable);

        public static Function Ctg(Function inner) => Cot(inner);



        public static Exp Exp(string variable) => Exp(Symbol(variable));

        public static Exp Exp(Symbol variable) => (Exp)Exp((Function)variable);

        public static Function Exp(Function inner) => _functionFactory(inner, () => new Exp(inner));



        public static Ln Ln(string variable) => Ln(Symbol(variable));

        public static Ln Ln(Symbol variable) => (Ln)Ln((Function)variable);

        public static Function Ln(Function inner)
        {
            if (inner == Functions.Constant.E) { return 1; }
            if (inner is Exp e) { return e.Inner; }
            if (inner is Power pw) { return pw.Exponent * Ln(pw.Inner); }
            else { return _functionFactory(inner, () => new Ln(inner)); }
        }



        public static Log Log(string variable, Constant @base) => Log(Symbol(variable), @base);

        public static Log Log(Symbol variable, Constant @base) => (Log)Log((Function)variable, @base);

        public static Function Log(Function inner, Constant @base)
        {
            if (inner == @base) { return 1; }
            else if (@base == Functions.Constant.E) { return Ln(inner); }
            else if (@base == 10) { return Log10(inner); }
            else if (@base == 2) { return Log2(inner); }
            else if (inner is Exponentiation e && e.Base == @base) { return e.Exponent; }
            else { return _functionFactory(inner, @base, () => new Log(inner, @base)); }
        }



        public static Log10 Log10(string variable) => Log10(Symbol(variable));

        public static Log10 Log10(Symbol variable) => (Log10)Log10((Function)variable);

        public static Function Log10(Function inner)
        {
            if (inner == 10) { return 1; }
            else if (inner is Exponentiation e && e.Base == 10) { return e.Exponent; }
            else if (inner is Power pw) { return pw.Exponent * Log10(pw.Inner); }
            else { return _functionFactory(inner, () => new Log10(inner)); }
        }



        public static Log2 Log2(string variable) => Log2(Symbol(variable));

        public static Log2 Log2(Symbol variable) => (Log2)Log2((Function)variable);

        public static Function Log2(Function inner)
        {
            if (inner == 2) { return 1; }
            else if (inner is Exponentiation e && e.Base == 2) { return e.Exponent; }
            else if (inner is Power pw) { return pw.Exponent * Log2(pw.Inner); }
            else { return _functionFactory(inner, () => new Log2(inner)); }
        }



        public static Power Power(string variable, Constant exponent) => Power(Symbol(variable), exponent);

        public static Power Power(Symbol variable, Constant exponent) => (Power)Power((Function)variable, exponent);

        public static Function Power(Function inner, Constant exponent)
        {
            if (exponent == 0) { return inner != 0 ? 1 : throw new ArithmeticException(); }
            else if (exponent == 1) { return inner; }
            else if ((1 / exponent) % 1 == 0) { return Root(inner, 1 / exponent); }
            else if (inner is not Functions.Symbol && inner is Monomial m && exponent % 1 == 0 && exponent > 0) { return Monomial(m.Inner, Math.Pow(m.Coefficient, exponent), m.Exponent * exponent); }
            else if (inner is not Functions.Symbol && inner is Polynomial p && exponent % 1 == 0 && exponent > 0)
            {
                Polynomial result = Polynomial(inner.Variable, new Constant[] { 1 });
                for (int i = 0; i < exponent; i++) { result = (Polynomial)(result * inner); }
                return result;
            }
            else if (inner is Root r)
            {
                if (r.Degree == exponent)
                {
                    if (r.Degree % 2 == 0) { return r.Inner; }   // TODO : Exception when inner function is negative?
                    else { return r.Inner; }
                }
                else if (r.Degree % exponent == 0)
                {
                    Constant newDegree = r.Degree / exponent;
                    if (r.Degree % 2 == 0 && newDegree % 2 != 0) { return Root(r.Inner, newDegree); }   // TODO : Exception when inner function is negative?
                    else { return Root(r.Inner, newDegree); }
                }
                else if (exponent % r.Degree == 0)
                {
                    Constant newExp = exponent / r.Degree;
                    if (r.Degree % 2 == 0) { return Abs(Power(r.Inner, newExp)); }   // TODO : Exception when inner function is negative?
                    else { return Abs(Power(r.Inner, newExp)); }   // Outer Abs will be simplifyed in case of exponent of Power is even.
                }
                else { return Power(r.Inner, exponent / r.Degree); }
            }
            else if (inner is Exp e) { return Exp(exponent * e.Inner); }
            else if (inner is Power pw)
            {
                if (pw.Exponent % 1 == 0 && exponent % 1 == 0) { return Power(pw.Inner, pw.Exponent * exponent); }
                else if ((1 / pw.Exponent) % 1 == 0) { return Power(Root(pw.Inner, 1 / pw.Exponent), exponent); }
                else { return Power(pw.Inner, pw.Exponent * exponent); }   // TODO : Rework and handle case where inner and/or outer Power is combination of Power and Root. It requires the use of Rational class for rational numbers.
            }
            else if (inner is Abs abs && exponent % 2 == 0) { return Power(abs.Inner, exponent); }
            else { return _functionFactory(inner, exponent, () => new Power(inner, exponent)); }
        }



        public static Root Root(string variable, Constant degree) => Root(Symbol(variable), degree);

        public static Root Root(Symbol variable, Constant degree) => (Root)Root((Function)variable, degree);

        public static Function Root(Function inner, Constant degree)
        {
            if (degree == 0) { throw new ArithmeticException(); }
            else if (degree == 1) { return inner; }
            else if ((1 / degree) % 1 == 0) { return Power(inner, 1 / degree); }
            else if (inner is Root r)
            {
                if (r.Degree % 1 == 0 && degree % 1 == 0) { return Root(r.Inner, r.Degree * degree); }
                else if ((1 / r.Degree) % 1 == 0) { return Root(Power(r.Inner, 1 / r.Degree), degree); }
                else { return Root(r.Inner, r.Degree * degree); }   // TODO : Rework and handle case where inner and/or outer Root is combination of Power and Root. It requires the use of Rational class for rational numbers.
            }
            else if (inner is Exp e) { return Exp(e.Inner / degree); }
            else if (inner is Power pw)
            {
                if (pw.Exponent == degree) { return degree % 2 == 0 ? Abs(pw.Inner) : pw.Inner; }
                else if (degree % pw.Exponent == 0)
                {
                    Constant newDegree = degree / pw.Exponent;
                    if (degree % 2 == 0 && pw.Exponent % 2 == 0) { return Root(Abs(pw.Inner), newDegree); }
                    else { return Root(pw.Inner, newDegree); }
                }
                else if (pw.Exponent % degree == 0)
                {
                    Constant newExp = pw.Exponent / degree;
                    if (degree % 2 == 0 && newExp % 2 != 0) { return Abs(Power(pw.Inner, newExp)); }
                    else { return Power(pw.Inner, newExp); }
                }
                else { return Power(pw.Inner, pw.Exponent / degree); }
            }
            else { return _functionFactory(inner, () => new Root(inner, degree)); }
        }



        public static Sin Sin(string variable) => Sin(Symbol(variable));

        public static Sin Sin(Symbol variable) => (Sin)Sin((Function)variable);

        public static Function Sin(Function inner) => inner is Negation n ? -Sin(n.Inner) : _functionFactory(inner, () => new Sin(inner));



        public static Sqrt Sqrt(string variable) => Sqrt(Symbol(variable));

        public static Sqrt Sqrt(Symbol variable) => (Sqrt)Sqrt((Function)variable);

        public static Function Sqrt(Function inner)
        {
            if (inner is Root r)
            {
                if (r.Degree % 1 == 0) { return Root(r.Inner, 2 * r.Degree); }
                else if ((1 / r.Degree) % 1 == 0) { return Sqrt(Power(r.Inner, 1 / r.Degree)); }
                else { return Root(r.Inner, 2 * r.Degree); }   // TODO : Rework and handle case where inner Root is combination of Power and Root. It requires the use of Rational class for rational numbers.
            }
            else if (inner is Exp e) { return Exp(e.Inner / 2); }
            else if (inner is Power pw)
            {
                if (pw.Exponent == 2) { return Abs(pw.Inner); }
                else if ((1 / pw.Exponent) % 1 == 0) { return Root(pw.Inner, 2 / pw.Exponent); }
                else if (pw.Exponent % 2 == 0) { return Abs(Power(pw.Inner, pw.Exponent / 2)); }   // Outer Abs will be simplifyed in case of exponent of Power is even.
                else { return Power(pw.Inner, pw.Exponent / 2); }
            }
            else { return _functionFactory(inner, () => new Sqrt(inner)); }
        }



        public static Tan Tan(string variable) => Tan(Symbol(variable));

        public static Tan Tan(Symbol variable) => (Tan)Tan((Function)variable);

        public static Function Tan(Function inner) => inner is Negation n ? -Tan(n.Inner) : _functionFactory(inner, () => new Tan(inner));

        public static Tan Tg(string variable) => Tan(variable);

        public static Tan Tg(Symbol variable) => Tan(variable);

        public static Function Tg(Function inner) => Tan(inner);
        #endregion


        #region Other functions
        public static Constant Constant(double value) => _functionFactory((object)value, () => new Constant(value));


        public static Function Exponentiation(Function @base, Function exponent)
        {
            if (@base is Constant cBase && exponent is Constant cExponent) { return Math.Pow(cBase, cExponent); }
            else if (@base == Functions.Constant.E) { return Exp(exponent); }
            else if (exponent is Constant c) { return Power(@base, c); }
            else { return _functionFactory(@base, exponent, false, () => new Exponentiation(@base, exponent)); }
        }


        public static Monomial Monomial(string variable, Constant coefficient, Constant exponent) => Monomial(Symbol(variable), coefficient, exponent);

        public static Monomial Monomial(Symbol variable, Constant coefficient, Constant exponent) => (Monomial)Monomial((Function)variable, coefficient, exponent);

        public static Function Monomial(Function inner, Constant coefficient, Constant exponent) => _functionFactory(inner, coefficient, exponent, false, () => new Monomial(inner, coefficient, exponent));


        public static Function Negation(Function inner)
        {
            if (inner is Constant c) { return -c.Value; }
            else if (inner is Negation n) { return n.Inner; }
            else if (inner is Sum s) { return (-s.Left) + (-s.Right); }
            else if (inner is Polynomial p) { return Polynomial(p.Inner, p.Coeffs.Select((Constant c) => -c), true); }
            // TODO : Handle cases where inner is Product or Quotient.
            else { return _functionFactory(inner, () => new Negation(inner)); }
        }


        // TODO : Add Partial() factory.


        public static Polynomial Polynomial(string variable, params Constant[] coeffs) => Polynomial(Symbol(variable), (IEnumerable<Constant>)coeffs);

        public static Polynomial Polynomial(string variable, IEnumerable<Constant> coeffs, bool reversed = false) => Polynomial(Symbol(variable), coeffs, reversed);

        public static Polynomial Polynomial(Symbol variable, params Constant[] coeffs) => Polynomial(variable, (IEnumerable<Constant>)coeffs);

        public static Polynomial Polynomial(Symbol variable, IEnumerable<Constant> coeffs, bool reversed = false) => (Polynomial)Polynomial((Function)variable, coeffs, reversed);

        public static Polynomial Polynomial(params Monomial[] monomials) => Polynomial((IEnumerable<Monomial>)monomials);

        public static Polynomial Polynomial(IEnumerable<Monomial> monomials)
        {
            Symbol variable = monomials.Aggregate(Functions.Symbol.ANY, (Symbol acc, Monomial curr) => acc | curr.Variable);
            if (!monomials.All((Monomial m) => m.Variable == variable)) { throw new Exception(); }   // TODO : Specify exception type.
            monomials = monomials.OrderBy((Monomial m) => m.Exponent);
            int maxPow = (int)monomials.Last().Exponent;
            if (monomials.All((Monomial m) => m.Exponent == maxPow)) { return Monomial(monomials.First().Variable, monomials.Sum((Monomial m) => m.Coefficient), maxPow); }
            Constant[] coeffs = Enumerable.Repeat(Functions.Constant.Zero, maxPow + 1).ToArray();
            foreach (Monomial m in monomials)
            {
                coeffs[(int)m.Exponent] += m.Coefficient;
            }
            int key = new HashCodeCombiner().AddType(typeof(Polynomial)).Add(variable).AddEnumerable(coeffs).Combine();
            return Properties.UseCaching ? (Polynomial)_cache.GetOrAdd(key, (_) => new Polynomial(variable, coeffs, true)) : new Polynomial(variable, coeffs, true);
        }

        public static Function Polynomial(Function inner, params Constant[] coeffs) => Polynomial(inner, (IEnumerable<Constant>)coeffs);

        public static Function Polynomial(Function inner, IEnumerable<Constant> coeffs, bool reversed = false)
        {
            if (coeffs.Count((Constant c) => c != 0) == 1) 
            {
                (Constant coeff, int exponent) = coeffs.Select((Constant c, int num) => (c, num)).Single(((Constant c, int _) t) => t.c != 0);
                return Monomial(inner, coeff, exponent);
            }
            if (!reversed) { coeffs = coeffs.Reverse(); }
            int key = new HashCodeCombiner().AddType(typeof(Polynomial)).Add(inner).AddEnumerable(coeffs).Combine();
            return Properties.UseCaching ? _cache.GetOrAdd(key, (_) => new Polynomial(inner, coeffs, true)) : new Polynomial(inner, coeffs, true);
        }


        public static Function Product(Function left, Function right)
        {
            if (left == 0 || right == 0) { return 0; }
            else if (left == 1) { return right; }
            else if (right == 1) { return left; }
            else if (left == -1) { return -right; }
            else if (right == -1) { return -left; }
            else if (left is Constant cLeft && right is Constant cRight) { return cLeft.Value * cRight.Value; }
            else if (left is Negation nLeft && right is Negation nRight) { return nLeft.Inner * nRight.Inner; }
            else if (left is Monomial mLeft && mLeft.Coefficient < 0 && right is Negation nRight1) { return (-mLeft) * nRight1.Inner; }
            else if (left is Negation nLeft1 && right is Monomial mRight && mRight.Coefficient < 0) { return nLeft1.Inner * (-mRight); }

            else if (left is Constant cLeft1 && right is Power pRight1 && pRight1.Inner is Symbol) { return Monomial(pRight1.Inner, cLeft1, pRight1.Exponent); }
            else if (left is Power pLeft1 && pLeft1.Inner is Symbol && right is Constant cRight1) { return Monomial(pLeft1.Inner, cRight1, pLeft1.Exponent); }
            else if (left is Symbol sLeft2 && right is Power pRight2 && pRight2.Inner == sLeft2) { return Power(sLeft2, pRight2.Exponent + 1); }
            else if (left is Power pLeft2 && right is Symbol sRight2 && pLeft2.Inner == sRight2) { return Power(sRight2, pLeft2.Exponent + 1); }

            else if (left is Constant cLeft3 && right is Monomial mRight3) { return Monomial(mRight3.Inner, cLeft3 * mRight3.Coefficient, mRight3.Exponent); }
            else if (left is Monomial mLeft3 && right is Constant cRight3) { return Monomial(mLeft3.Inner, cRight3 * mLeft3.Coefficient, mLeft3.Exponent); }
            else if (left is Symbol sLeft4 && right is Monomial mRight4 && mRight4.Inner == sLeft4) { return Monomial(sLeft4, mRight4.Coefficient, mRight4.Exponent + 1); }
            else if (left is Monomial mLeft4 && right is Symbol sRight4 && mLeft4.Inner == sRight4) { return Monomial(sRight4, mLeft4.Coefficient, mLeft4.Exponent + 1); }

            else if (left is Monomial mLeft5 && right is Monomial mRight5 && mLeft5.Inner == mRight5.Inner) { return Monomial(mLeft5.Inner, mLeft5.Coefficient * mRight5.Coefficient, mLeft5.Exponent + mRight5.Exponent); }

            else if (left is Polynomial pLeft6 && right is Polynomial pRight6 && left.Inner == right.Inner) { return _polyProduct(pLeft6, pRight6); }
            else if (left is Polynomial pLeft7 && right is Power pwRight7 && left.Inner == right.Inner) { return _polyProduct(pLeft7, (Polynomial)pwRight7); }
            else if (left is Power pwLeft8 && right is Polynomial pRight8 && left.Inner == right.Inner) { return _polyProduct((Polynomial)pwLeft8, pRight8); }

            else if (left is Exp && right is Exp) { return Exp(left.Inner + right.Inner); }
            else if (left is Power pwLeft && right is Power pwRight && pwLeft.Inner == pwRight.Inner) { return Power(pwLeft.Inner, pwLeft.Exponent + pwRight.Exponent); }
            else if (left is Constant cLeft5 && right is Log lRight) { return Log(Power(lRight.Inner, cLeft5), lRight.Base); }
            else if (left is Log lLeft && right is Constant cRight5) { return Log(Power(lLeft.Inner, cRight5), lLeft.Base); }
            else { return _functionFactory(left, right, true, () => new Product(left, right)); }
        }


        public static Function Quotient(Function left, Function right)
        {
            if (right == 0) { throw new DivideByZeroException(); }
            else if (left == 0) { return 0; }
            else if (right == 1) { return left; }
            else if (right == -1) { return -left; }
            else if (left == right) { return 1; }
            else if (left is Constant cLeft && right is Constant cRight) { return cLeft.Value / cRight.Value; }
            else if (left is Negation nLeft && right is Negation nRight) { return nLeft.Inner / nRight.Inner; }
            else if (left is Monomial mLeft && mLeft.Coefficient < 0 && right is Negation nRight1) { return (-mLeft) / nRight1.Inner; }
            else if (left is Negation nLeft1 && right is Monomial mRight && mRight.Coefficient < 0) { return nLeft1.Inner / (-mRight); }

            else if (left is Power pLeft1 && pLeft1.Inner is Symbol && right is Constant cRight1) { return Monomial(pLeft1.Inner, 1 / cRight1, pLeft1.Exponent); }
            else if (left is Symbol sLeft2 && right is Power pRight2 && pRight2.Inner == sLeft2) { return Power(sLeft2, 1 - pRight2.Exponent); }
            else if (left is Power pLeft2 && right is Symbol sRight2 && pLeft2.Inner == sRight2) { return Power(sRight2, pLeft2.Exponent - 1); }

            else if (left is Monomial mLeft3 && right is Constant cRight3) { return Monomial(mLeft3.Inner, mLeft3.Coefficient / cRight3, mLeft3.Exponent); }
            else if (left is Monomial mLeft4 && right is Symbol sRight4 && mLeft4.Inner == sRight4 && mLeft4.Exponent >= 1)
            {
                return Monomial(sRight4, mLeft4.Coefficient, mLeft4.Exponent - 1);
            }

            else if (left is Monomial mLeft5 && right is Monomial mRight5 && mLeft5.Inner == mRight5.Inner && mLeft5.Exponent >= mRight5.Exponent)
            {
                return Monomial(mLeft5.Inner, mLeft5.Coefficient / mRight5.Coefficient, mLeft5.Exponent - mRight5.Exponent);
            }

            else if (left is Polynomial pLeft6 && right is Constant cRight7) { return Polynomial(pLeft6.Inner, pLeft6.Coeffs.Select((Constant c) => c / cRight7), true); }

            else if (left is Quotient qLeft && right is Quotient qRight) { return (qLeft.Left * qRight.Right) / (qLeft.Right * qRight.Left); }
            else if (left is Quotient qLeft1) { return qLeft1.Left / (qLeft1.Right * right); }
            else if (right is Quotient qRight1) { return (left * qRight1.Right) / qRight1.Left; }
            else if (left is Product pLeft)
            {
                if (pLeft.Left == right) { return pLeft.Right; }
                else if (pLeft.Right == right) { return pLeft.Left; }
                else if (right is Product pRight)
                {
                    if (pLeft.Left == pRight.Left) { return pLeft.Right / pRight.Right; }
                    else if (pLeft.Left == pRight.Right) { return pLeft.Right / pRight.Left; }
                    else if (pLeft.Right == pRight.Left) { return pLeft.Left / pRight.Right; }
                    else if (pLeft.Right == pRight.Right) { return pLeft.Right / pRight.Right; }
                    else { return _functionFactory(left, right, false, () => new Quotient(left, right)); }
                }
                else { return _functionFactory(left, right, false, () => new Quotient(left, right)); }
            }
            else if (left is Exp && right is Exp) { return Exp(left.Inner - right.Inner); }
            else if (left is Power pwLeft && right is Power pwRight && pwLeft.Inner == pwRight.Inner) { return Power(pwLeft.Inner, pwLeft.Exponent - pwRight.Exponent); }
            else if (left is Log l && right is Constant c) { return Log(Root(l.Inner, c), l.Base); }
            else { return _functionFactory(left, right, false, () => new Quotient(left, right)); }
        }


        public static Function Sum(Function left, Function right)
        {
            if (left is Negation nLeft && nLeft.Inner == right) { return 0; }
            else if (right is Negation nRight && nRight.Inner == left) { return 0; }
            else if (left == 0) { return right; }
            else if (right == 0) { return left; }
            else if (left is Constant cLeft && right is Constant cRight) { return cLeft.Value + cRight.Value; }
            else if (left is Monomial mLeft && right is Monomial mRight && mLeft.Inner == mRight.Inner)
            {
                if (mLeft.Exponent == mRight.Exponent) { return Monomial(mLeft.Inner, mLeft.Coefficient + mRight.Coefficient, mLeft.Exponent); }
                else { return Polynomial(mLeft, mRight); }
            }
            
            else if (left is Polynomial pLeft1 && right is Polynomial pRight1 && left.Inner == right.Inner) { return _polySum(pLeft1, pRight1); }
            else if (left is Polynomial pLeft2 && right is Power pwRight2 && left.Inner == right.Inner) { return _polySum(pLeft2, (Polynomial)pwRight2); }
            else if (left is Power pwLeft3 && right is Polynomial pRight3 && left.Inner == right.Inner) { return _polySum((Polynomial)pwLeft3, pRight3); }

            else if (left is Log lLeft && right is Log lRight && lLeft.Base == lRight.Base) { return Log(lLeft.Inner * lRight.Inner, lLeft.Base); }
            else if (left is Log lLeft1 && right is Negation nRight1 && nRight1.Inner is Log lRight1 && lLeft1.Base == lRight1.Base) { return Log(lLeft1.Inner / lRight1.Inner, lLeft1.Base); }
            else if (left is Negation nLeft2 && nLeft2.Inner is Log lLeft2 && right is Log lRight2 && lLeft2.Base == lRight2.Base) { return Log(lRight2.Inner / lLeft2.Inner, lLeft2.Base); }
            else { return _functionFactory(left, right, true, () => new Sum(left, right)); }
        }


        public static Symbol Symbol(string symbol)
        {
            if (Properties.UseCaching)
            {
                Symbol result = _symbolCache.GetOrAdd(symbol, (_) => new Symbol(symbol));
                if (result is SymbolicConstant) { throw new Exception(); }   // TODO : Specify exception type.
                else { return result; }
            }
            else { return new Symbol(symbol); }
        }

        public static SymbolicConstant SymbolicConstant(string symbol, Constant value)
        {
            if (Properties.UseCaching)
            {
                Symbol result = _symbolCache.GetOrAdd(symbol, (_) => new SymbolicConstant(symbol, value));
                if (result is not SymbolicConstant sc) { throw new Exception(); }   // TODO : Specify exception type.
                else { return sc; }
            }
            else { return new SymbolicConstant(symbol, value); }
        }
        #endregion

        public static void ClearCache() => _cache.Clear();

        private static Function _functionFactory<T>(Function inner, Func<T> constructor) where T : Function =>
            _functionFactory(inner, Enumerable.Empty<Function>(), true, constructor);

        private static Function _functionFactory<T>(Function inner, Function param, Func<T> constructor) where T : Function =>
            _functionFactory(inner, new Function[] { param }, true, constructor);

        private static Function _functionFactory<T>(Function inner, Function firstParam, Function secondParam, bool isCommutative, Func<T> constructor) where T : Function =>
            _functionFactory(inner, new Function[] { firstParam, secondParam }, isCommutative, constructor);

        private static Function _functionFactory<T>(Function inner, IEnumerable<Function> @params, bool isCommutative, Func<T> constructor) where T : Function
        {
            HashCodeCombiner combiner = new HashCodeCombiner().AddType(typeof(T)).Add(inner);
            if (isCommutative) { combiner = combiner.Add(@params); }
            else
            {
                foreach (Function f in @params) { combiner.Add(f); }
            }
            int key = combiner.Combine();
            return Properties.UseCaching ? _cache.GetOrAdd(key, (_) => constructor()) : constructor();
        }

        private static T _functionFactory<T>(object param, Func<T> constructor) where T : Function
        {
            int key = new HashCodeCombiner().AddType(typeof(T)).Add(param).Combine();
            return Properties.UseCaching ? (T)_cache.GetOrAdd(key, (_) => constructor()) : constructor();
        }

        private static T _functionFactory<T>(Function firstParam, Function secondParam, bool isCommutative, Func<T> constructor) where T : Function
        {
            HashCodeCombiner combiner = new HashCodeCombiner().AddType(typeof(T));
            combiner = isCommutative ? combiner.Add(firstParam, secondParam) : combiner.Add(firstParam).Add(secondParam);
            int key = combiner.Combine();
            return Properties.UseCaching ? (T)_cache.GetOrAdd(key, (_) => constructor()) : constructor();
        }

        private static Polynomial _polySum(Polynomial left, Polynomial right)
        {
            int count1 = left.Coeffs.Count();
            int count2 = right.Coeffs.Count();
            IEnumerable<Constant> newCoeffs = left.Coeffs.Zip(right.Coeffs, (Constant c1, Constant c2) => c1 + c2);
            if (count1 > count2) { newCoeffs = newCoeffs.Concat(left.Coeffs.Skip(count2)); }
            else { newCoeffs = newCoeffs.Concat(right.Coeffs.Skip(count1)); }
            return Polynomial(left.Variable | right.Variable, newCoeffs, true);
        }

        private static Polynomial _polyProduct(Polynomial left, Polynomial right)
        {
            int leftLen = left.Coeffs.Count();
            int rightLen = right.Coeffs.Count();
            Constant[] newCoeffs = Enumerable.Repeat(Functions.Constant.Zero, leftLen + rightLen - 1).ToArray();
            int index1 = 0;
            foreach (double c1 in left.Coeffs)
            {
                int index2 = 0;
                foreach (double c2 in right.Coeffs)
                {
                    newCoeffs[index1 + index2] += c1 * c2;
                    index2++;
                }
                index1++;
            }
            return Polynomial(left.Variable | right.Variable, newCoeffs, true);
        }
    }
}
