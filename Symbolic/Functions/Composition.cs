namespace Symbolic.Functions
{
    public class Composition : Function
    {
        public Function Outer { get; init; }
        public Function Inner { get; init; }

        public Composition(Function outer, Function inner) => (Outer, Inner) = (outer, inner);

        public override Function Diff() => Outer.Diff().ApplyTo(Inner) * Inner.Diff();

        public override bool Equals(Function? other) => other is Composition c && c.Outer.Equals(Outer) && c.Inner.Equals(Inner);

        public override double GetValue(double variableValue) => Outer.GetValue(Inner.GetValue(variableValue));

        public override Function ApplyTo(Function inner) => Outer.ApplyTo(Inner.ApplyTo(inner));

        public override Function WithVariable(Symbol newVariable) => Outer.ApplyTo(Inner.WithVariable(newVariable));

        public override string ToString(string? inner) => Outer.ToString(Inner.ToString(inner));

        public override string ToString() => Outer.ToString(Inner.ToString());
    }
}
