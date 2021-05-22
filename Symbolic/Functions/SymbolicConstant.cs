using Symbolic.Utils;

using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions
{
    public class SymbolicConstant : Constant
    {
        public static new readonly SymbolicConstant E = SymbolicConstant("e", Constant.E);
        public static new readonly SymbolicConstant PI = SymbolicConstant("pi", Constant.PI);

        public string StrSymbol { get; init; }

        public SymbolicConstant(string strSymbol, Constant value) : base(value)
        {
            StrSymbol = strSymbol;
            PriorityWhenInner = Priorities.NonNegativeConstant;
            PriorityWhenOuter = Priorities.Constant;
            _prefixStringRepr = $"( {strSymbol} )";
        }

        public override bool Equals(Function? other) => other is SymbolicConstant sc && StrSymbol == sc.StrSymbol && Value == sc.Value;

        public override string ToPrefixString() => $"( {StrSymbol} )";

        protected override string _toString() => StrSymbol;

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => combiner.Add(StrSymbol).Add(Value);
    }
}
