﻿using System;
using System.Linq;

using Symbolic.Utils;
using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public abstract class Function : IEquatable<Function>
    {
        public Symbol Variable { get; init; }
        public bool HasAllIntegralsKnown { get; init; } = false;

        protected int? _hashCode;

        protected Function() => Variable = Symbol.ANY;

        public Function(Symbol variable) => Variable = variable;

        public abstract double GetValue(double variableValue);

        public virtual Function Diff(Symbol variable) => variable == Variable ? _diff(variable) : 0;

        public virtual Function Integrate(Symbol variable) => variable == Variable ? _integrate(variable) : this * variable;

        public virtual double Integrate(Symbol variable, Constant lowerBound, Constant upperBound)
        {
            Function antideriv = Integrate(variable);
            return antideriv.GetValue(upperBound) - antideriv.GetValue(lowerBound);
        }

        public virtual Function Negate() => Negation(this);

        public virtual Function Add(Function other)
        {
            if (this == -other) { return 0; }
            else if (other == 0) { return this; }
            else if (other is Partial p) 
            { 
                return new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Add(t.func), t.leftBound, t.rightBound)));
            }
            else { return Sum(this, other); }
        }

        public virtual Function Subtract(Function other)
        {
            if (this == other) { return 0; }
            else if (other == 0) { return this; }
            else { return this.Add(other.Negate()); }
        }

        public virtual Function Multiply(Function other)
        {
            if (other == -1) { return this.Negate(); }
            else if (other == 0) { return 0; }
            else if (other == 1) { return this; }
            else if (other is Partial p)
            {
                return new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Multiply(t.func), t.leftBound, t.rightBound)));
            }
            else { return Product(this, other); }
        }

        public virtual Function Divide(Function other)
        {
            if (this == other) { return 1; }
            else if (other == -1) { return this.Negate(); }
            else if (other == 1) { return this; }
            else if (other is Partial p)
            {
                return new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Divide(t.func), t.leftBound, t.rightBound)));
            }
            else { return Quotient(this, other); }
        }

        public virtual Function Raise(Function other)
        {
            if (other == 0 && this == 0) { throw new ArithmeticException(); }
            else if (other == 1) { return this; }
            else if (other is Constant c) { return Power(this, c); }
            else if (other is Partial p)
            {
                return new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Raise(t.func), t.leftBound, t.rightBound)));
            }
            return Exponentiation(this, other);
        }

        public virtual Function ApplyTo(Function inner) => inner switch
                                                           {
                                                               Constant c => GetValue(c),
                                                               Symbol s => WithVariable(s),
                                                               Partial p => new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.ApplyTo(t.func), t.leftBound, t.rightBound))),
                                                               _ => Composition(this, inner)
                                                           };

        public abstract Function WithVariable(Symbol newVariable);
        
        public abstract bool Equals(Function? other);

        public override bool Equals(object? obj) => obj is Function f && this.Equals(f);

        public override int GetHashCode() => _hashCode ??= _addHashCodeParams(_addHashCodeVariable(_addHashCodeType(new HashCodeCombiner()))).Combine();

        public virtual string ToString(string inner) => $"Unknown function of {inner}";

        public override string ToString() => ToString(Variable.ToString());

        public virtual string ToPrefixString(string inner) => $"func {inner}";

        public virtual string ToPrefixString() => ToPrefixString(Variable.ToPrefixString());

        protected abstract Function _diff(Symbol variable);

        protected abstract Function _integrate(Symbol variable);

        protected virtual HashCodeCombiner _addHashCodeType(HashCodeCombiner combiner) => combiner.AddType(this.GetType());

        protected virtual HashCodeCombiner _addHashCodeVariable(HashCodeCombiner combiner) => combiner.Add(Variable);

        protected virtual HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner;

        #region Operators
        public static bool operator ==(Function? left, Function? right) => (left is null && right is null) || (left is not null && left.Equals(right));

        public static bool operator !=(Function? left, Function? right) => !(left == right);

        public static Function operator -(Function inner) => inner.Negate();

        public static Function operator +(Function left, Function right) => left.Add(right);

        public static Function operator -(Function left, Function right) => left.Subtract(right);

        public static Function operator *(Function left, Function right) => left.Multiply(right);

        public static Function operator /(Function left, Function right) => left.Divide(right);

        public static Function operator ^(Function left, Function right) => left.Raise(right);

        public static implicit operator Function(double d) => (Constant)d;
        #endregion
    }
}
