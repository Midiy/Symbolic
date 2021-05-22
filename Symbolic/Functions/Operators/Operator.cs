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

        public override string ToString(Priorities outerPriority) => outerPriority >= PriorityWhenInner ? $"({_toString()})" : _toString();

        public override string ToPrefixString() => $"{_stringRepr} {Left.ToPrefixString()} {Right.ToPrefixString()}";

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
