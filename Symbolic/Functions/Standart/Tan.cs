using System;

namespace Symbolic.Functions.Standart
{
    public class Tan : Function
    {
        public Tan(Symbol variable) : base(variable) { }

        public override double GetValue(double variableValue) => Math.Tan(variableValue);

        public override Tan WithVariable(Symbol newVariable) => new Tan(newVariable);

        public override bool Equals(Function? other) => other is Tan && other.Variable == Variable;

        public override string ToString(string inner) => $"tan({inner})";
        
        protected override Function _diff(Symbol _) => 1 / (new Cos(Variable) ^ 2);

        protected override Function _integrate(Symbol _) => throw new NotImplementedException();   // TODO : Implement after adding Functions.Standart.Abs : -ln(cos(|x|)).
    }
}
