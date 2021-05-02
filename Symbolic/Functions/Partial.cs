using System;
using System.Collections.Generic;
using System.Linq;

using Symbolic.Utils;

namespace Symbolic.Functions
{
    public class Partial : Function
    {
        public IEnumerable<(Function func, Constant leftBound, Constant rightBound)> Parts { get; init; }

        public Partial(IEnumerable<(Function func, Constant leftBound, Constant rightBound)> parts)
        {
            Symbol variable = parts.Aggregate(Symbol.ANY, (Symbol acc, (Function func, Constant, Constant) curr) => acc | curr.func.Variable);
            if (!parts.All(((Function func, Constant, Constant) t) => t.func.Variable == variable)) { throw new Exception(); }    // TODO : Specify exception type.
            Inner = variable;
            Variable = variable;
            Parts = _checkIfSimular(parts.OrderBy(((Function, Constant lowerBound, Constant) t) => t.lowerBound));
            HasAllIntegralsKnown = parts.All(((Function func, Constant, Constant) t) => t.func.HasAllIntegralsKnown);
        }

        public Partial(params (Function func, Constant leftBound, Constant rightBound)[] parts) : this((IEnumerable<(Function func, Constant leftBound, Constant rightBound)>)parts) { }

        public override Partial Negate() => _transform((Function f) => f.Negate());

        public override Function Add(Function other) => other is Partial p ? _transform(p, (Function left, Function right) => left.Add(right)) : _transform((Function f) => f.Add(other));

        public override Function Subtract(Function other) => other is Partial p ? _transform(p, (Function left, Function right) => left.Subtract(right)) : _transform((Function f) => f.Subtract(other));

        public override Function Multiply(Function other) => other is Partial p ? _transform(p, (Function left, Function right) => left.Multiply(right)) : _transform((Function f) => f.Multiply(other));

        public override Function Divide(Function other) => other is Partial p ? _transform(p, (Function left, Function right) => left.Divide(right)) : _transform((Function f) => f.Divide(other));

        public override Function Raise(Function other) => other is Partial p ? _transform(p, (Function left, Function right) => left.Raise(right)) : _transform((Function f) => f.Raise(other));

        public override Function ApplyTo(Function inner) => inner switch
                                                               {
                                                                   Constant c => GetValue(c),
                                                                   Partial p  => _transform(p, (Function outer, Function inner) => outer.ApplyTo(inner)),
                                                                   _          => _transform((Function f) => f.ApplyTo(inner))
                                                               };

        public override string ToPrefixString() => $"partial {Variable}";

        protected override double _getValue(double variableValue)
        {
            Function? part = Parts.FirstOrDefault(((Function func, Constant leftBound, Constant rightBound) t) => t.leftBound < variableValue && variableValue < t.rightBound).func;
            if (part == default) { throw new ArgumentOutOfRangeException(); }
            else { return part.GetValue(variableValue); }
        }

        protected override Function _applyTo(Function inner) => ApplyTo(inner);

        protected override bool _equals(Function? other) => other is Partial p && Parts.SequenceEqual(p.Parts);

        // TODO : String representation.
        protected override string _toString() => $"partial({Variable})";

        protected override Partial _diff(Symbol variable) => _transform((Function f) => f.Diff(variable));

        protected override Partial _integrate(Symbol variable) => _transform((Function f) => f.Integrate(variable));

        protected override HashCodeCombiner _addInnerHashCode(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner)
        {
            return combiner.Add(Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => new HashCodeCombiner(2).Add(t.func)
                                                                                                                                    .Add(t.leftBound)
                                                                                                                                    .Add(t.rightBound)
                                                                                                                                    .Combine())
                                     .ToArray());
        }

        private Partial _transform(Func<Function, Function> transformer) => new Partial(Parts.Select(((Function func, Constant leftBound, Constant rightBound) t) => (transformer(t.func), t.leftBound, t.rightBound)));

        private Partial _transform(Partial other, Func<Function, Function, Function> transformer)
        {
            IEnumerable<(Function func, Constant leftBound, Constant rightBound)> result = Enumerable.Empty<(Function func, Constant leftBound, Constant rightBound)>();
            Func<Constant, Constant, Constant, Constant, bool> isIntersects = (Constant leftBound1, Constant rightBound1, Constant leftBound2, Constant rightBound2) => leftBound1 < rightBound2 && leftBound2 < rightBound1;
            foreach ((Function func, Constant leftBound, Constant rightBound) in Parts)
            {
                IEnumerable<(Function func, Constant leftBound, Constant rightBound)> toApply = other.Parts.Where(((Function _, Constant leftBound, Constant rightBound) t) => isIntersects(leftBound, rightBound, t.leftBound, t.rightBound));
                toApply = toApply.Select(((Function func, Constant leftBound, Constant rightBound) t) => (t.func, (Constant)Math.Max(leftBound, t.leftBound), (Constant)Math.Min(rightBound, t.rightBound)));
                result = result.Concat(toApply.Select(((Function func, Constant leftBound, Constant rightBound) t) => (transformer(func, t.func), t.leftBound, t.rightBound)));
            }
            return new Partial(result);
        }

        private static IEnumerable<(Function func, Constant leftBound, Constant rightBound)> _checkIfSimular(IEnumerable<(Function func, Constant leftBound, Constant rightBound)> parts)
        {
            if (!parts.Any()) { return parts; }
            LinkedList<(Function func, Constant leftBound, Constant rightBound)> result = new();
            (Function currFunc, Constant currLeftBound, Constant currRightBound) = parts.First();
            foreach ((Function func, Constant leftBound, Constant rightBound) in parts.Skip(1))
            {
                if (func == currFunc && leftBound == currRightBound) { currRightBound = rightBound; }
                else 
                {
                    result.AddLast((currFunc, currLeftBound, currRightBound));
                    (currFunc, currLeftBound, currRightBound) = (func, leftBound, rightBound);
                }
                result.AddLast((currFunc, currLeftBound, currRightBound));
            }
            return result;
        }
    }
}
