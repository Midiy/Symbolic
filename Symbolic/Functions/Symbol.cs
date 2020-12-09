namespace Symbolic.Functions
{
    public class Symbol : Function
    {
        public static readonly Symbol ANY = new Symbol("ANY") { _isAny = true };

        public string StrSymbol { get; init; }

        private bool _isAny { get; init; } = false;

        public Symbol(string strSymbol)
        {
            StrSymbol = strSymbol;
            Variable = this;
        }

        public override double GetValue(double variableValue) => variableValue;

        public override Function ApplyTo(Function inner) => inner;

        public override Symbol WithVariable(Symbol newVariable) => newVariable;

        public override bool Equals(Function? other) => other is Symbol s && (s.StrSymbol == StrSymbol || _isAny || s._isAny);

        public override string ToString(string? inner) => inner;

        public override string ToString() => StrSymbol;

        protected override Constant _diff(Symbol _) => 1;
    }
}
