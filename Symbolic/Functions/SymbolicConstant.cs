using System;
using System.Collections.Generic;

using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public class SymbolicConstant : Symbol, IComparable<Constant>, IComparable<double>
    {
        public static readonly SymbolicConstant E = SymbolicConstant("e", Constant.E);
        public static readonly SymbolicConstant PI = SymbolicConstant("pi", Constant.PI);

        public Constant Value { get; init; }

        public SymbolicConstant(string strSymbol, Constant value) : base(strSymbol)
        {
            Variable = ANY;
            Value = value;
            PriorityWhenInner = Priorities.NonNegativeConstant;
            PriorityWhenOuter = Priorities.Constant;
        }

        public override double GetValue(double _) => Value;

        public override double GetValue(Dictionary<Symbol, double> variableValues) => Value;

        public override Constant Diff(Symbol _) => 0;

        public override Function Integrate(Symbol variable) => this * variable;

        public override Function ApplyTo(Function _) => this;

        public override Function ApplyTo(Dictionary<Symbol, Function> replacements) => this;

        public override bool Equals(Function? other) => other is SymbolicConstant sc && StrSymbol == sc.StrSymbol && Value == sc.Value;

        public override bool StrictEquals(Symbol other) => Equals(other);

        public int CompareTo(Constant? other) => Value.CompareTo(other?.Value);

        public int CompareTo(double value) => Value.CompareTo(value);

        public override string ToPrefixString() => $"( {StrSymbol} )";

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.Add(StrSymbol).Add(Value);
    }
}
