using System;

namespace Symbolic.Functions
{
    public abstract class Function : IEquatable<Function>
    {
        public Symbol? Variable { get; init; }

        public abstract double GetValue(double variableValue);

        public abstract Function Diff();

        public virtual Function Negate() => new Negation(this);

        public virtual Function Add(Function other) => new Sum(this, other);

        public virtual Function Subtract(Function other) => this == other ? 0 : this.Add(other.Negate());

        public virtual Function Multiply(Function other) => new Product(this, other);

        public virtual Function Divide(Function other) => this == other ? 1 : new Quotient(this, other);

        public virtual Function ApplyTo(Function inner) => inner is Constant c ? this.GetValue(c) : new Composition(this, inner);

        public virtual Function Raise(Function other) => other is Constant c ? new Standart.Power(c).ApplyTo(other) : new Exponentiation(this, other);
        
        public abstract bool Equals(Function? other);

        public override bool Equals(object? obj) => obj is Function f && this.Equals(f);

        public virtual string ToString(string? inner) => $"Unknown function of {inner}";

        public override string ToString() => ToString(Variable?.ToString());

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
