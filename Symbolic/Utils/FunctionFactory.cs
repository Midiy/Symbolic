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
        private static ConcurrentDictionary<int, Function> _cache = new();
        private static ConcurrentDictionary<string, Symbol> _symbolCache = new();

        #region Standart functions
        public static Abs Abs(string variable) => Abs(Symbol(variable));

        public static Abs Abs(Symbol variable) => (Abs)Abs((Function)variable);

        public static Function Abs(Function inner) => _functionFactory(inner, () => new Abs(inner));



        public static Cos Cos(string variable) => Cos(Symbol(variable));

        public static Cos Cos(Symbol variable) => (Cos)Cos((Function)variable);

        public static Function Cos(Function inner) => _functionFactory(inner, () => new Cos(inner));



        public static Cot Cot(string variable) => Cot(Symbol(variable));

        public static Cot Cot(Symbol variable) => (Cot)Cot((Function)variable);

        public static Function Cot(Function inner) => _functionFactory(inner, () => new Cot(inner));

        public static Cot Ctg(string variable) => Cot(variable);

        public static Cot Ctg(Symbol variable) => Cot(variable);

        public static Function Ctg(Function inner) => Cot(inner);



        public static Exp Exp(string variable) => Exp(Symbol(variable));

        public static Exp Exp(Symbol variable) => (Exp)Exp((Function)variable);

        public static Function Exp(Function inner) => _functionFactory(inner, () => new Exp(inner));



        public static Ln Ln(string variable) => Ln(Symbol(variable));

        public static Ln Ln(Symbol variable) => (Ln)Ln((Function)variable);

        public static Function Ln(Function inner) => _functionFactory(inner, () => new Ln(inner));



        public static Log Log(string variable, Constant @base) => Log(Symbol(variable), @base);

        public static Log Log(Symbol variable, Constant @base) => (Log)Log((Function)variable, @base);

        public static Function Log(Function inner, Constant @base) => _functionFactory(inner, @base, () => new Log(inner, @base));



        public static Log10 Log10(string variable) => Log10(Symbol(variable));

        public static Log10 Log10(Symbol variable) => (Log10)Log10((Function)variable);

        public static Function Log10(Function inner) => _functionFactory(inner, () => new Log10(inner));



        public static Log2 Log2(string variable) => Log2(Symbol(variable));

        public static Log2 Log2(Symbol variable) => (Log2)Log2((Function)variable);

        public static Function Log2(Function inner) => _functionFactory(inner, () => new Log2(inner));



        public static Power Power(string variable, Constant exponent) => Power(Symbol(variable), exponent);

        public static Power Power(Symbol variable, Constant exponent) => (Power)Power((Function)variable, exponent);

        public static Function Power(Function inner, Constant exponent) => _functionFactory(inner, exponent, () => new Power(inner, exponent));



        public static Root Root(string variable, Constant degree) => Root(Symbol(variable), degree);

        public static Root Root(Symbol variable, Constant degree) => (Root)Root((Function)variable, degree);

        public static Function Root(Function inner, Constant degree) => _functionFactory(inner, () => new Root(inner, degree));



        public static Sin Sin(string variable) => Sin(Symbol(variable));

        public static Sin Sin(Symbol variable) => (Sin)Sin((Function)variable);

        public static Function Sin(Function inner) => _functionFactory(inner, () => new Sin(inner));



        public static Sqrt Sqrt(string variable) => Sqrt(Symbol(variable));

        public static Sqrt Sqrt(Symbol variable) => (Sqrt)Sqrt((Function)variable);

        public static Function Sqrt(Function inner) => _functionFactory(inner, () => new Sqrt(inner));



        public static Tan Tan(string variable) => Tan(Symbol(variable));

        public static Tan Tan(Symbol variable) => (Tan)Tan((Function)variable);

        public static Function Tan(Function inner) => _functionFactory(inner, () => new Tan(inner));

        public static Tan Tg(string variable) => Tan(variable);

        public static Tan Tg(Symbol variable) => Tan(variable);

        public static Function Tg(Function inner) => Tan(inner);
        #endregion

        #region Other functions
        public static Constant Constant(double value) => _functionFactory((object)value, () => new Constant(value));

        
        public static Exponentiation Exponentiation(Function @base, Function exponent) => _functionFactory(@base, exponent, false, () => new Exponentiation(@base, exponent));


        public static Monomial Monomial(string variable, Constant coefficient, Constant exponent) => Monomial(Symbol(variable), coefficient, exponent);

        public static Monomial Monomial(Symbol variable, Constant coefficient, Constant exponent) => (Monomial)Monomial((Function)variable, coefficient, exponent);

        public static Function Monomial(Function inner, Constant coefficient, Constant exponent) => _functionFactory(inner, coefficient, exponent, false, () => new Monomial(inner, coefficient, exponent));


        public static Function Negation(Function inner) => _functionFactory(inner, () => new Negation(inner));


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
            Constant[] coeffs = new Constant[maxPow + 1];
            int position = 0;
            foreach (Monomial m in monomials)
            {
                while (m.Exponent > position) { coeffs[position++] = 0; }
                coeffs[position++] = m.Coefficient;
            }
            int key = new HashCodeCombiner().AddType(typeof(Polynomial)).Add(variable).AddEnumerable(coeffs).Combine();
            return Properties.UseCaching ? (Polynomial)_cache.GetOrAdd(key, (_) => new Polynomial(variable, coeffs, true)) : new Polynomial(variable, coeffs, true);
        }

        public static Function Polynomial(Function inner, params Constant[] coeffs) => Polynomial(inner, (IEnumerable<Constant>)coeffs);

        public static Function Polynomial(Function inner, IEnumerable<Constant> coeffs, bool reversed = false)
        {
            if (!reversed) { coeffs = coeffs.Reverse(); }
            int key = new HashCodeCombiner().AddType(typeof(Polynomial)).Add(inner).AddEnumerable(coeffs).Combine();
            return Properties.UseCaching ? _cache.GetOrAdd(key, (_) => new Polynomial(inner, coeffs, true)) : new Polynomial(inner, coeffs, true);
        }


        public static Product Product(Function left, Function right) => _functionFactory(left, right, true, () => new Product(left, right));


        public static Quotient Quotient(Function left, Function right) => _functionFactory(left, right, false, () => new Quotient(left, right));


        public static Sum Sum(Function left, Function right) => _functionFactory(left, right, true, () => new Sum(left, right));


        public static Symbol Symbol(string symbol) => Properties.UseCaching ? _symbolCache.GetOrAdd(symbol, (_) => new Symbol(symbol)) : new Symbol(symbol);
        #endregion

        public static void ClearCache() => _cache.Clear();

        private static Function _functionFactory<T>(Function inner, Func<T> constructor) where T : Function =>
            _functionFactory(inner, Enumerable.Empty<Function>(), true, constructor);

        private static Function _functionFactory<T>(Function inner, Function param, Func<T> constructor) where T : Function =>
            _functionFactory(inner, new Function[] { param }, true, constructor);

        private static Function _functionFactory<T>(Function inner, Function firstParam, Function secondParam, bool isCommutative, Func<T> constructor) where T : Function =>
            _functionFactory(inner, new Function[] { firstParam, secondParam }, isCommutative, constructor);

        private  static Function _functionFactory<T>(Function inner, IEnumerable<Function> @params, bool isCommutative, Func<T> constructor) where T : Function
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
    }
}
