using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Root : Power
    {
        public Constant Degree { get; init; }

        public Root(Function inner, Constant degree) : base(inner, 1 / degree) => Degree = degree;

        public override string ToPrefixString() => $"root {Inner.ToPrefixString()} {Degree.ToPrefixString()}";

        protected override Function _applyTo(Function inner) => Root(inner, Degree);
    }
}
