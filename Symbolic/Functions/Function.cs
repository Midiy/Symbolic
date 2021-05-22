using System;
using System.Collections.Generic;
using System.Linq;

using Symbolic.Utils;
using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public abstract class Function : IEquatable<Function>
    {
        public Function Inner { get; init; }
        public Symbol Variable { get; init; }
        public bool HasAllIntegralsKnown { get; init; } = false;
        public Priorities PriorityWhenInner { get; init; } = Priorities.Min;
        public Priorities PriorityWhenOuter { get; init; } = Priorities.Max;

        protected int? _hashCode;

        protected Function() => Inner = Variable = Symbol.ANY;

        public Function(Function inner)
        {
            Inner = inner;
            Variable = Inner.Variable;
        }

        public virtual double GetValue(double variableValue) => _getValue(Inner.GetValue(variableValue));

        public virtual double GetValue(Symbol variable, double value) => GetValue((variable, value));

        public virtual double GetValue(params (Symbol variable, double value)[] variableValues)
            => GetValue(new Dictionary<Symbol, double>(variableValues.Select(((Symbol key, double value) t) => new KeyValuePair<Symbol, double>(t.key, t.value))));

        public virtual double GetValue(IEnumerable<(Symbol variable, double value)> variableValues)
            => GetValue(new Dictionary<Symbol, double>(variableValues.Select(((Symbol key, double value) t) => new KeyValuePair<Symbol, double>(t.key, t.value))));

        public virtual double GetValue(Dictionary<Symbol, double> variableValues) => _getValue(Inner.GetValue(variableValues));

        public virtual Function Diff(Symbol variable) => variable == Variable ? _diff(variable) * Inner.Diff(variable) : 0;

        public virtual Function Integrate(Symbol variable)
        {
            if (variable == Variable)
            {
                if (Inner is Symbol) { return _integrate(variable); }
                else { throw new NotImplementedException(); }
            }
            else { return this * variable; }
        }

        public virtual double Integrate(Symbol variable, Constant lowerBound, Constant upperBound)
        {
            Function antideriv = Integrate(variable);
            return antideriv.GetValue(upperBound) - antideriv.GetValue(lowerBound);
        }

        public virtual Function Integrate(Symbol variable, Function lowerBound, Function upperBound)
        {
            Function antideriv = Integrate(variable);
            return antideriv.ApplyTo(variable, upperBound) - antideriv.ApplyTo(variable, lowerBound);
        }

        public virtual Function Negate() => Negation(this);

        public virtual Function Add(Function other) => other is Partial p ? new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Add(t.func), t.leftBound, t.rightBound)))
                                                                          : Sum(this, other);

        public virtual Function Subtract(Function other) => this.Add(-other);

        public virtual Function Multiply(Function other) => other is Partial p ? new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Multiply(t.func), t.leftBound, t.rightBound)))
                                                                               : Product(this, other);

        public virtual Function Divide(Function other) => other is Partial p ? new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Divide(t.func), t.leftBound, t.rightBound)))
                                                                             : Quotient(this, other);

        public virtual Function Raise(Function other) => other is Partial p ? new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.Raise(t.func), t.leftBound, t.rightBound)))
                                                                            : Quotient(this, other);
        
        public virtual Function ApplyTo(Function inner) => inner switch
            {
                Partial p => new Partial(p.Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (this.ApplyTo(t.func), t.leftBound, t.rightBound))),
                _ => _applyTo(Inner.ApplyTo(inner))
            };

        public virtual Function ApplyTo(Symbol variable, Function inner) => ApplyTo((variable, inner));

        public virtual Function ApplyTo(params (Symbol variable, Function inner)[] replacements)
            => ApplyTo(new Dictionary<Symbol, Function>(replacements.Select(((Symbol key, Function value) t) => new KeyValuePair<Symbol, Function>(t.key, t.value))));

        public virtual Function ApplyTo(IEnumerable<(Symbol variable, Function inner)> replacements)
            => ApplyTo(new Dictionary<Symbol, Function>(replacements.Select(((Symbol key, Function value) t) => new KeyValuePair<Symbol, Function>(t.key, t.value))));

        public virtual Function ApplyTo(Dictionary<Symbol, Function> replacements) => _applyTo(Inner.ApplyTo(replacements));

        public virtual bool Equals(Function? other) => other is not null && Inner == other.Inner && _equals(other);

        public override bool Equals(object? obj) => obj is Function f && this.Equals(f);

        public override int GetHashCode() => _hashCode ??= _addParamsHashCode(_addInnerHashCode(_addTypeHashCode(new HashCodeCombiner()))).Combine();

        public override string ToString() => ToString(Priorities.Min);

        public virtual string ToString(Priorities outerPriority) => outerPriority > PriorityWhenInner ? $"({_toString()})" : _toString();

        public virtual string ToPrefixString() => $"func {Inner.ToPrefixString()}";

        protected abstract double _getValue(double variableValue);

        protected abstract Function _applyTo(Function inner);

        protected virtual bool _equals(Function other) => this.GetType() == other.GetType();

        protected abstract Function _diff(Symbol variable);

        protected abstract Function _integrate(Symbol variable);

        protected virtual HashCodeCombiner _addTypeHashCode(HashCodeCombiner combiner) => combiner.AddType(this.GetType());

        protected virtual HashCodeCombiner _addInnerHashCode(HashCodeCombiner combiner) => combiner.Add(Inner);

        protected virtual HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner;

        protected virtual string _toString() => $"Unknown function of {Inner.ToString(PriorityWhenOuter)}";

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
