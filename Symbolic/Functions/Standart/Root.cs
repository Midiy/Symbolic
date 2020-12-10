namespace Symbolic.Functions.Standart
{
    public class Root : Power
    {
        public Constant Degree { get => 1 / Exponent; }

        public Root(Symbol variable, Constant degree) : base(variable, 1 / degree) { }

        public override Root WithVariable(Symbol newVariable) => new Root(newVariable, Degree);
    }
}
