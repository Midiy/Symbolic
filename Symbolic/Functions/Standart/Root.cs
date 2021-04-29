using static Symbolic.Utils.FunctionFactory;

namespace Symbolic.Functions.Standart
{
    public class Root : Power
    {
        public Constant Degree;

        public Root(Symbol variable, Constant degree) : base(variable, 1 / degree) => Degree = degree;

        public override Root WithVariable(Symbol newVariable) => Root(newVariable, Degree);

        public override string ToPrefixString(string inner) => $"root {inner} {Degree.ToPrefixString()}";
    }
}
