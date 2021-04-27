using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Symbolic.Functions;
using Symbolic.Functions.Standart;

namespace Symbolic.Utils
{
    public static class FunctionFactory
    {
        private static ConcurrentDictionary<int, Function> _cache = new();
        private static ConcurrentDictionary<string, Symbol> _symbolCache = new();

        #region Standart functions
        public static Abs Abs(string variable) => Abs(Symbol(variable));

        public static Abs Abs(Symbol variable) => (Abs)_functionFactory(variable, (Symbol variable) => new Abs(variable));

        public static Function Abs(Function inner) => _functionFactory(inner, (Symbol variable) => new Abs(variable));



        public static Cos Cos(string variable) => Cos(Symbol(variable));

        public static Cos Cos(Symbol variable) => (Cos)_functionFactory(variable, (Symbol variable) => new Cos(variable));

        public static Function Cos(Function inner) => _functionFactory(inner, (Symbol variable) => new Cos(variable));



        public static Cot Cot(string variable) => Cot(Symbol(variable));

        public static Cot Cot(Symbol variable) => (Cot)_functionFactory(variable, (Symbol variable) => new Cot(variable));

        public static Function Cot(Function inner) => _functionFactory(inner, (Symbol variable) => new Cot(variable));

        public static Cot Ctg(string variable) => Cot(variable);

        public static Cot Ctg(Symbol variable) => Cot(variable);

        public static Function Ctg(Function inner) => Cot(inner);



        public static Exp Exp(string variable) => Exp(Symbol(variable));

        public static Exp Exp(Symbol variable) => (Exp)_functionFactory(variable, (Symbol variable) => new Exp(variable));

        public static Function Exp(Function inner) => _functionFactory(inner, (Symbol variable) => new Exp(variable));



        public static Ln Ln(string variable) => Ln(Symbol(variable));

        public static Ln Ln(Symbol variable) => (Ln)_functionFactory(variable, (Symbol variable) => new Ln(variable));

        public static Function Ln(Function inner) => _functionFactory(inner, (Symbol variable) => new Ln(variable));



        public static Log Log(string variable, Constant @base) => Log(Symbol(variable), @base);

        public static Log Log(Symbol variable, Constant @base) => (Log)_functionFactory(variable, @base, (Symbol variable) => new Log(variable, @base));

        public static Function Log(Function inner, Constant @base) => _functionFactory(inner, @base, (Symbol variable) => new Log(variable, @base));



        public static Log10 Log10(string variable) => Log10(Symbol(variable));

        public static Log10 Log10(Symbol variable) => (Log10)_functionFactory(variable, (Symbol variable) => new Log10(variable));

        public static Function Log10(Function inner) => _functionFactory(inner, (Symbol variable) => new Log10(variable));



        public static Log2 Log2(string variable) => Log2(Symbol(variable));

        public static Log2 Log2(Symbol variable) => (Log2)_functionFactory(variable, (Symbol variable) => new Log2(variable));

        public static Function Log2(Function inner) => _functionFactory(inner, (Symbol variable) => new Log2(variable));



        public static Power Power(string variable, Constant exponent) => Power(Symbol(variable), exponent);

        public static Power Power(Symbol variable, Constant exponent) => (Power)_functionFactory(variable, exponent, (Symbol variable) => new Power(variable, exponent));

        public static Function Power(Function inner, Constant exponent) => _functionFactory(inner, exponent, (Symbol variable) => new Power(variable, exponent));



        public static Root Root(string variable, Constant degree) => Root(Symbol(variable), degree);

        public static Root Root(Symbol variable, Constant degree) => (Root)_functionFactory(variable, (Symbol variable) => new Root(variable, degree));

        public static Function Root(Function inner, Constant degree) => _functionFactory(inner, (Symbol variable) => new Root(variable, degree));



        public static Sin Sin(string variable) => Sin(Symbol(variable));

        public static Sin Sin(Symbol variable) => (Sin)_functionFactory(variable, (Symbol variable) => new Sin(variable));

        public static Function Sin(Function inner) => _functionFactory(inner, (Symbol variable) => new Sin(variable));



        public static Sqrt Sqrt(string variable) => Sqrt(Symbol(variable));

        public static Sqrt Sqrt(Symbol variable) => (Sqrt)_functionFactory(variable, (Symbol variable) => new Sqrt(variable));

        public static Function Sqrt(Function inner) => _functionFactory(inner, (Symbol variable) => new Sqrt(variable));



        public static Tan Tan(string variable) => Tan(Symbol(variable));

        public static Tan Tan(Symbol variable) => (Tan)_functionFactory(variable, (Symbol variable) => new Tan(variable));

        public static Function Tan(Function inner) => _functionFactory(inner, (Symbol variable) => new Tan(variable));

        public static Tan Tg(string variable) => Tan(variable);

        public static Tan Tg(Symbol variable) => Tan(variable);

        public static Function Tg(Function inner) => Tan(inner);
        #endregion

        #region Other functions
        public static Composition Composition(Function outer, Function inner) => _functionFactory(outer, inner, false, () => new Composition(outer, inner));


        public static Constant Constant(double value) => _functionFactory((object)value, () => new Constant(value));

        
        public static Exponentiation Exponentiation(Function @base, Function exponent) => _functionFactory(@base, exponent, false, () => new Exponentiation(@base, exponent));


        public static Monomial Monomial(string variable, Constant coefficient, Constant exponent) => Monomial(Symbol(variable), coefficient, exponent);

        public static Monomial Monomial(Symbol variable, Constant coefficient, Constant exponent) => (Monomial)_functionFactory(variable, coefficient, exponent, false, (Symbol variable) => new Monomial(variable, coefficient, exponent));

        public static Function Monomial(Function inner, Constant coefficient, Constant exponent) => _functionFactory(inner, coefficient, exponent, false, (Symbol variable) => new Monomial(variable, coefficient, exponent));


        public static Negation Negation(Function inner) => _functionFactory(inner, () => new Negation(inner));


        // TODO : Add Partial() factory.


        public static Polynomial Polynomial(string variable, params Constant[] coeffs) => Polynomial(Symbol(variable), (IEnumerable<Constant>)coeffs);

        public static Polynomial Polynomial(string variable, IEnumerable<Constant> coeffs, bool reversed = false) => Polynomial(Symbol(variable), coeffs, reversed);

        public static Polynomial Polynomial(Symbol variable, params Constant[] coeffs) => Polynomial(variable, (IEnumerable<Constant>)coeffs);

        public static Polynomial Polynomial(Symbol variable, IEnumerable<Constant> coeffs, bool reversed = false)
        {
            if (!reversed) { coeffs = coeffs.Reverse(); }
            int key = new HashCodeCombiner().AddType(typeof(Polynomial)).Add(variable).AddEnumerable(coeffs).Combine();
            return Properties.UseCaching ? (Polynomial)_cache.GetOrAdd(key, (_) => new Polynomial(variable, coeffs, true)) : new Polynomial(variable, coeffs, true);
        }

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
            return Properties.UseCaching ? _cache.GetOrAdd(key, (_) => Polynomial(Functions.Symbol.ANY, coeffs, true).ApplyTo(inner))
                                         : Polynomial(Functions.Symbol.ANY, coeffs, true).ApplyTo(inner);
        }


        public static Product Product(Function left, Function right) => _functionFactory(left, right, true, () => new Product(left, right));


        public static Quotient Quotient(Function left, Function right) => _functionFactory(left, right, false, () => new Quotient(left, right));


        public static Sum Sum(Function left, Function right) => _functionFactory(left, right, true, () => new Sum(left, right));


        public static Symbol Symbol(string symbol) => Properties.UseCaching ? _symbolCache.GetOrAdd(symbol, (_) => new Symbol(symbol)) : new Symbol(symbol);
        #endregion

        public static void ClearCache() => _cache.Clear();

        private static Function _functionFactory<T>(Function inner, Func<Symbol, T> constructor) where T : Function =>
            _functionFactory(inner, Enumerable.Empty<Function>(), true, constructor);

        private static Function _functionFactory<T>(Function inner, Function param, Func<Symbol, T> constructor) where T : Function =>
            _functionFactory(inner, new Function[] { param }, true, constructor);

        private static Function _functionFactory<T>(Function inner, Function firstParam, Function secondParam, bool isCommutative, Func<Symbol, T> constructor) where T : Function =>
            _functionFactory(inner, new Function[] { firstParam, secondParam }, isCommutative, constructor);

        private  static Function _functionFactory<T>(Function inner, IEnumerable<Function> @params, bool isCommutative, Func<Symbol, T> constructor) where T : Function
        {
            HashCodeCombiner combiner = new HashCodeCombiner().AddType(typeof(T)).Add(inner);
            if (isCommutative) { combiner = combiner.Add(@params); }
            else
            {
                foreach (Function f in @params) { combiner.Add(f); }
            }
            int key = combiner.Combine();
            if (inner is Symbol variable)
            {
                return Properties.UseCaching ? _cache.GetOrAdd(key, (_) => constructor(variable)) : constructor(variable);
            }
            else
            {
                return Properties.UseCaching ? _cache.GetOrAdd(key, (_) => _functionFactory(Functions.Symbol.ANY, @params, isCommutative, constructor).ApplyTo(inner))
                                             : _functionFactory(Functions.Symbol.ANY, @params, isCommutative, constructor).ApplyTo(inner);
            }
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
