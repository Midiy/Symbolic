namespace Symbolic.Functions
{
    public class Symbol : Function
    {
        public string StrSymbol { get; init; }

        public Symbol(string strSymbol)
        {
            StrSymbol = strSymbol;
            Variable = this;
        }

        public override double GetValue(double variableValue) => variableValue;

        public override Constant Diff() => 1;

        public override Function ApplyTo(Function inner) => inner;

        public override Symbol WithVariable(Symbol newVariable) => newVariable;

        public override bool Equals(Function? other) => other is Symbol s && s.StrSymbol == StrSymbol;

        public override string ToString(string? inner) => inner;

        public override string ToString() => StrSymbol;
    }
}
