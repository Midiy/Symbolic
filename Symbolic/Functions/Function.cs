using System;

namespace Symbolic.Functions
{
    public abstract class Function : IEquatable<Function>
    {
        public Symbol? Variable { get; init; }

        protected Function() { }

        public Function(Symbol? variable) => Variable = variable;

        public abstract double GetValue(double variableValue);

        public virtual Function Diff(Symbol variable) => variable == Variable ? _diff(variable) : 0;

        public virtual Function Negate() => new Negation(this);

        public virtual Function Add(Function other) => new Sum(this, other);

        public virtual Function Subtract(Function other) => this == other ? 0 : this.Add(other.Negate());

        public virtual Function Multiply(Function other) => new Product(this, other);

        public virtual Function Divide(Function other) => this == other ? 1 : new Quotient(this, other);

        public virtual Function ApplyTo(Function inner) => inner switch
                                                           {
                                                               Constant c => GetValue(c),
                                                               Symbol s => WithVariable(s),
                                                               _ => new Composition(this, inner)
                                                           };

        public virtual Function Raise(Function other) => other is Constant c ? new Standart.Power(null, c).ApplyTo(this) : new Exponentiation(this, other);

        public abstract Function WithVariable(Symbol newVariable);
        
        public abstract bool Equals(Function? other);

        public override bool Equals(object? obj) => obj is Function f && this.Equals(f);

        public virtual string ToString(string? inner) => $"Unknown function of {inner}";

        public override string ToString() => ToString(Variable?.ToString());

        protected abstract Function _diff(Symbol variable);

        #region Operators
        public static bool operator ==(Function left, Function right) => left.Equals(right);

        public static bool operator !=(Function left, Function right) => !left.Equals(right);

        public static Function operator -(Function inner) => inner.Negate();

        public static Function operator +(Function left, Function right) => left.Add(right);

        public static Function operator -(Function left, Function right) => left.Subtract(right);

        public static Function operator *(Function left, Function right) => left.Multiply(right);

        public static Function operator /(Function left, Function right) => left.Divide(right);

        public static Function operator ^(Function left, Function right) => left.Raise(right);

        public static implicit operator Function(double d) => new Constant(d);
        #endregion
    }
}
