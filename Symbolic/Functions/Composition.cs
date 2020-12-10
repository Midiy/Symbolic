namespace Symbolic.Functions
{
    public class Composition : Function
    {
        public Function Outer { get; init; }
        public Function Inner { get; init; }

        public Composition(Function outer, Function inner) : base(inner.Variable)
        {
            (Outer, Inner) = (outer, inner);
            HasAllIntegralsKnown = Inner is Symbol && Outer.HasAllIntegralsKnown;
        }

        public override bool Equals(Function? other) => other is Composition c && c.Outer.Equals(Outer) && c.Inner.Equals(Inner);

        public override double GetValue(double variableValue) => Outer.GetValue(Inner.GetValue(variableValue));

        public override Function ApplyTo(Function inner) => Outer.ApplyTo(Inner.ApplyTo(inner));

        public override Function WithVariable(Symbol newVariable) => Outer.ApplyTo(Inner.WithVariable(newVariable));

        public override string ToString(string inner) => Outer.ToString(Inner.ToString(inner));

        public override string ToString() => Outer.ToString(Inner.ToString());

        protected override Function _diff(Symbol variable) => Outer.Diff(Outer.Variable).ApplyTo(Inner) * Inner.Diff(variable);

        protected override Function _integrate(Symbol variable)
        {
            if (Inner is Symbol) { return Outer.Integrate(variable); }
            else { throw new System.NotImplementedException(); }
        }
    }
}
