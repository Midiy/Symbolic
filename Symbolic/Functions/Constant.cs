﻿using System;

namespace Symbolic.Functions
{
    public class Constant : Monomial, IComparable<Constant>, IComparable<double>
    {
        public static readonly Constant E = Math.E;
        public static readonly Constant PI = Math.PI;
        public static readonly Constant PositiveInfinity = double.PositiveInfinity;
        public static readonly Constant NegativeInfinity = double.NegativeInfinity;

        public double Value { get; init; }

        public Constant(double value)
        {
            Variable = Symbol.ANY;
            Value = value;
            Coefficient = this;
            Exponent = value == 0 ? this : 0;
            Coeffs = new Constant[] { this };
        }

        public override double GetValue(double _) => Value;

        public override Constant Diff(Symbol _) => 0;

        public override Constant Negate() => -Value;

        public override Function Add(Function other)
        {
            if (Value == 0) { return other; }
            else if (other is Constant c) { return Value + c.Value; }
            else { return base.Add(other); }
        }

        public override Function Subtract(Function other)
        {
            if (Value == 0) { return other.Negate(); }
            else if (other is Constant c) { return Value - c.Value; }
            else { return base.Subtract(other);
}
        }

        public override Function Multiply(Function other)
        {
            if (Value == 0) { return 0; }
            else if (Value == 1) { return other; }
            else if (Value == -1) { return other.Negate(); }
            else if (other is Constant c) { return Value * c.Value; }
            else { return base.Multiply(other); }
        }

        public override Function Divide(Function other)
        {
            if (other is Constant c)
            {
                if (c == 0) { throw new DivideByZeroException(); }
                else { return Value / c.Value; }
            }
            else if (Value == 0) { return 0; }
            else { return base.Divide(other); }
        }

        public override Function Raise(Function other)
        {
            if (Value == 0) { return other == 0 ? throw new ArithmeticException() : 0; }
            else if (Value == 1) { return 1; }
            else if (other is Constant c) { return Math.Pow(this, c); }
            else { return base.Multiply(other); }
        }

        public override Function ApplyTo(Function _) => this;

        public override Function WithVariable(Symbol newVariable) => this;

        public int CompareTo(Constant? other) => Value.CompareTo(other?.Value);

        public int CompareTo(double value) => Value.CompareTo(value);

        public override bool Equals(Function? other) => other is Constant c && c.Value == Value;

        public override string ToString(string inner) => ToString();

        public override string ToString() => Value.ToString();

        protected override Function _diff(Symbol variable) => 0;

        protected override Function _integrate(Symbol variable) => this * variable;

        #region Operators
        public static Constant operator -(Constant inner) => -inner.Value;

        public static Constant operator +(Constant left, Constant right) => left.Value + right.Value;

        public static Constant operator -(Constant left, Constant right) => left.Value - right.Value;

        public static Constant operator *(Constant left, Constant right) => left.Value * right.Value;

        public static Constant operator /(Constant left, Constant right) => right == 0 ? throw new DivideByZeroException() : left.Value / right.Value;

        public static Constant operator ^(Constant left, Constant right) => Math.Pow(left.Value, right.Value);

        public static implicit operator double(Constant c) => c.Value;

        public static implicit operator Constant(double d) => new Constant(d);
        #endregion
    }
}
