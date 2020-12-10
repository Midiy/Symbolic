using System;

using Symbolic.Functions.Standart;

namespace Symbolic.Functions
{
    public class Constant : Function, IComparable<Constant>, IComparable<double>
    {
        public static readonly Constant E = Math.E;
        public static readonly Constant PI = Math.PI;

        public double Value { get; init; }

        public Constant(double value) : base(Symbol.ANY) => Value = value;

        public override double GetValue(double _) => Value;

        public override Constant Diff(Symbol _) => 0;

        public override Function Negate() => -Value;

        public override Function Add(Function other)
        {
            if (Value == 0) { return other; }
            else if (other is Constant c) { return Value + c.Value; }
            else if (other is Power pw &&  pw.Exponent % 1 == 0) { return ((Monomial)pw).Add(this); }
            else if (other is Symbol || other is Monomial || other is Polynomial) { return other.Add(this); }
            else { return base.Add(other); }
        }

        public override Function Subtract(Function other)
        {
            if (Value == 0) { return other.Negate(); }
            else if (other is Constant c) { return Value - c.Value; }
            else if (other is Power pw && pw.Exponent % 1 == 0) { return ((Monomial)pw).Negate().Add(this); }
            else if (other is Symbol || other is Monomial || other is Polynomial) { return other.Negate().Add(this); }
            else { return base.Subtract(other); }
        }

        public override Function Multiply(Function other)
        {
            if (Value == 0) { return 0; }
            else if (Value == 1) { return other; }
            else if (Value == -1) { return other.Negate(); }
            else if (other is Constant c) { return Value * c.Value; }
            else if (other is Power pw && pw.Exponent % 1 == 0) { return ((Monomial)pw).Multiply(this); }
            else if (other is Symbol || other is Monomial || other is Polynomial) { return other.Multiply(this); }
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

        public override Function ApplyTo(Function _) => this;

        public override Function WithVariable(Symbol newVariable) => this;

        public int CompareTo(Constant? other) => Value.CompareTo(other?.Value);

        public int CompareTo(double value) => Value.CompareTo(value);

        public override bool Equals(Function? other) => (other as Constant)?.Value == Value;

        public override string ToString(string? inner) => ToString();

        public override string ToString() => Value.ToString();

        protected override Function _diff(Symbol variable) => throw new NotImplementedException();

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
