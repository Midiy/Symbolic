using System;
using System.Collections.Generic;

using Symbolic.Utils;

namespace Symbolic.Functions.Operators
{
    public abstract class Operator : Function
    {
        public Function Left { get; init; }
        public Function Right { get; init; }

        protected bool _isCommutative { get; init; }
        protected string _stringRepr { get; init; }

        public Operator(Function left, Function right, bool isCommutative, string prefixRepr) 
            : base((left.Variable.StrictEquals(right.Variable) || left is Constant || right is Constant || left is SymbolicConstant || right is SymbolicConstant) ? left.Variable | right.Variable : Symbol.ANY)
        {
            Left = left;
            Right = right;
            _isCommutative = isCommutative;
            _stringRepr = prefixRepr;
        }

        // TODO : Rework.
        public override double GetValue(double variableValue) => Left.Variable == Right.Variable ? _getValue(variableValue) : throw new Exception();   // TODO : Specify exception type.

        public override double GetValue(Dictionary<Symbol, double> variableValues)
        {
            if (!Symbol.IsAny(Variable) && !variableValues.ContainsKey(Variable)) { throw new Exception(); }   // TODO : Specify exception type.
            return _getValue(Left.GetValue(variableValues), Right.GetValue(variableValues));
        }

        public override Function ApplyTo(Function inner) => Left.Variable == Right.Variable ? _applyTo(inner) : throw new Exception();   // TODO : Specify exception type.

        public override Function ApplyTo(Dictionary<Symbol, Function> replacements) => _applyTo(Left.ApplyTo(replacements), Right.ApplyTo(replacements));

        public override string ToString(Priorities outerPriority) => outerPriority >= PriorityWhenInner ? $"({_toString()})" : _toString();

        public override string ToPrefixString() => $"{_stringRepr} {Left.ToPrefixString()} {Right.ToPrefixString()}";

        protected override double _getValue(double variableValue) => _getValue(Left.GetValue(variableValue), Right.GetValue(variableValue));

        protected abstract double _getValue(double leftValue, double rightValue);

        protected override Function _applyTo(Function inner) => _applyTo(Left.ApplyTo(inner), Right.ApplyTo(inner));

        protected abstract Function _applyTo(Function left, Function right);

        protected override bool _equals(Function other) => other is Operator o &&
                                                           this.GetType() == other.GetType() &&
                                                           (
                                                               (Left == o.Left && Right == o.Right) ||
                                                               (_isCommutative && Left == o.Right && Right == o.Left)
                                                           );

        protected override HashCodeCombiner _addInnerHashCode(HashCodeCombiner combiner) => combiner;

        protected override HashCodeCombiner _addParamsHashCode(HashCodeCombiner combiner) => _isCommutative ? combiner.Add(Left, Right) : combiner.Add(Left).Add(Right);

        protected override string _toString()
        {
            string leftString = Left.ToString(Left.PriorityWhenInner == PriorityWhenOuter ? Priorities.Min : PriorityWhenOuter);
            string rightString = Right.ToString(_isCommutative && Right.PriorityWhenInner == PriorityWhenOuter ? Priorities.Min : PriorityWhenOuter);
            return $"{leftString} {_stringRepr} {rightString}";
        }
    }
}
