using System;
using System.Linq;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public class Constant : Monomial, IComparable<Constant>, IComparable<double>
    {
        // Shouldn't use implicit conversion operator here to avoid circular references.
        public static readonly Constant E                = Constant(Math.E);
        public static readonly Constant PI               = Constant(Math.PI);
        public static readonly Constant PositiveInfinity = Constant(double.PositiveInfinity);
        public static readonly Constant NegativeInfinity = Constant(double.NegativeInfinity);
        public static readonly Constant Zero             = Constant(0);
        public static readonly Constant One              = Constant(1);
        public static readonly Constant MinusOne         = Constant(-1);

        public double Value { get; init; }

        private string _prefixStringRepr;

        public Constant(double value)
        {
            Variable = Symbol.ANY;
            Value = value;
            Coefficient = this;
            Exponent = value == 0 ? this : 0;
            Coeffs = new Constant[] { this };

            bool signFlag = value < 0;
            string[] digits = Math.Abs(value).ToString().ToCharArray().Select((char c) => c.ToString()).ToArray();
            if (signFlag) { digits[0] = "-" + digits[0]; }
            _prefixStringRepr = $"( {string.Join(" ", digits)} )";
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

        public override string ToString(string _) => Value.ToString();

        public override string ToPrefixString(string _) => _prefixStringRepr;

        protected override Function _diff(Symbol variable) => 0;

        protected override Function _integrate(Symbol variable) => this * variable;

        protected override HashCodeCombiner _addHashCodeVariable(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addHashCodeParams(HashCodeCombiner combiner) => combiner.Add(Value);

        #region Operators
        public static Constant operator -(Constant inner) => -inner.Value;

        public static Constant operator +(Constant left, Constant right) => left.Value + right.Value;

        public static Constant operator -(Constant left, Constant right) => left.Value - right.Value;

        public static Constant operator *(Constant left, Constant right) => left.Value * right.Value;

        public static Constant operator /(Constant left, Constant right) => right == 0 ? throw new DivideByZeroException() : left.Value / right.Value;

        public static Constant operator ^(Constant left, Constant right) => Math.Pow(left.Value, right.Value);

        public static implicit operator double(Constant c) => c.Value;

        public static implicit operator Constant(double d) => d switch
                                                              {
                                                                  // Handling trivial and most common cases to reduce number of memory allocations and objects in heap.
                                                                  // (Not quite actual with caching by Symbols.Utils.FunctionFactory.)
                                                                  0 => Zero,
                                                                  1 => One,
                                                                  -1 => MinusOne,
                                                                  Math.E => E,
                                                                  Math.PI => PI,
                                                                  double.PositiveInfinity => PositiveInfinity,
                                                                  double.NegativeInfinity => NegativeInfinity,
                                                                  _ => Constant(d)
                                                              };
        #endregion
    }
}
