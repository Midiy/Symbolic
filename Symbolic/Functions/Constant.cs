using System;

namespace Symbolic.Functions
{
    public class Constant : Function
    {
        public double Value { get; init; }
        
        public Constant(double value) => Value = value;

        public override double GetValue(double _) => Value;

        public override Constant Diff() => 0;

        public override Function Negate() => -Value;

        public override Function Add(Function other)
        {
            if (Value == 0) { return other; }
            else if (other is Constant c) { return Value + c.Value; }
            else { return base.Add(other); }
        }

        public override Function Subtract(Function other)
        {
            if (Value == 0) { return other.Negate(); }
            else if (other is Constant c) { return Value + c.Value; }
            else { return base.Add(other); }
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
            if (Value == 0) { return 0; }
            else if (other is Constant c)
            {
                if (c == 0) { throw new System.DivideByZeroException(); }
                else { return Value / c.Value; }
            }
            else { return base.Add(other); }
        }

        public override Function ApplyTo(Function _) => this;

        public override bool Equals(Function? other) => (other as Constant)?.Value == Value;

        public override string ToString(string? inner) => ToString();

        public override string ToString() => Value.ToString();

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
