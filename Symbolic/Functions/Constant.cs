using System;
using System.Linq;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public class Constant : Monomial, IComparable<Constant>, IComparable<double>
    {
        // Shouldn't use implicit conversion operator here to avoid circular references.
        public static readonly Constant E                = Math.E;
        public static readonly Constant PI               = Math.PI;
        public static readonly Constant PositiveInfinity = double.PositiveInfinity;
        public static readonly Constant NegativeInfinity = double.NegativeInfinity;
        public static readonly Constant Zero             = 0;
        public static readonly Constant One              = 1;
        public static readonly Constant MinusOne         = -1;

        public double Value { get; init; }

        private string _prefixStringRepr;

        public Constant(double value)
        {
            Variable = Symbol.ANY;
            Value = value;
            Coefficient = this;
            Exponent = value == 0 ? this : 0;
            Coeffs = new Constant[] { this };
            PriorityWhenInner = value < 0 ? Priorities.NegativeConstant : Priorities.NonNegativeConstant;
            PriorityWhenOuter = Priorities.Constant;

            bool signFlag = value < 0;
            string[] digits = Math.Abs(value).ToString().ToCharArray().Select((char c) => c.ToString()).ToArray();
            if (signFlag) { digits[0] = "-" + digits[0]; }
            _prefixStringRepr = $"( {string.Join(" ", digits)} )";
        }

        public override bool Equals(Function? other) => other is Constant c && Value == c.Value;

        public int CompareTo(Constant? other) => Value.CompareTo(other?.Value);

        public int CompareTo(double value) => Value.CompareTo(value);

        public override string ToPrefixString() => _prefixStringRepr;

        protected override double _getValue(double _) => Value;

        protected override Function _applyTo(Function _) => this;

        protected override bool _equals(Function? other) => Equals(other);

        protected override Constant _diff(Symbol _) => 0;

        protected override Function _integrate(Symbol variable) => this * variable;

        protected override HashCodeCombiner _addInnerHashCode(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.Add(Value);

        protected override string _toString() => Value.ToString();

        #region Operators
        public static Constant operator -(Constant inner) => -inner.Value;

        public static Constant operator +(Constant left, Constant right) => left.Value + right.Value;

        public static Constant operator -(Constant left, Constant right) => left.Value - right.Value;

        public static Constant operator *(Constant left, Constant right) => left.Value * right.Value;

        public static Constant operator /(Constant left, Constant right) => right == 0 ? throw new DivideByZeroException() : left.Value / right.Value;

        public static Constant operator ^(Constant left, Constant right) => Math.Pow(left.Value, right.Value);

        public static implicit operator double(Constant c) => c.Value;

        public static implicit operator Constant(double d) => Constant(d);
        #endregion
    }
}
