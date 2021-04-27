using System;
using System.Collections.Generic;

namespace Symbolic.Utils
{
    public class HashCodeCombiner
    {
        private static int _typeCoeff = 41;
        private static int[] _coeffs = { 43, 47, 53, 59, 61, 67, 71, 73, 79 };

        private int _hashCode = 0;
        private int _coeffNumber;

        public HashCodeCombiner(int coeffOffset = 0) { _coeffNumber = coeffOffset; }

        public HashCodeCombiner AddType(Type type)
        {
            if (_coeffNumber >= _coeffs.Length) { throw new Exception("Too many hash code parts!"); }   // TODO : Specify exception type.
            if (type != null)
            {
                unchecked { _hashCode += type.GetHashCode() * _typeCoeff; }
            }
            return this;
        }

        public HashCodeCombiner Add(params object?[]? parts) => Add((IEnumerable<object?>?)parts);

        public HashCodeCombiner Add(IEnumerable<object?>? parts)
        {
            if (_coeffNumber >= _coeffs.Length) { throw new Exception("Too many hash code parts!"); }   // TODO : Specify exception type.
            if (parts != null)
            {
                unchecked
                {
                    int result = 0;
                    foreach (object? o in parts) 
                    {
                        if (o != null) { result += o.GetHashCode(); }
                    }
                    _hashCode += result * _coeffs[_coeffNumber];
                }
            }
            _coeffNumber++;
            return this;
        }

        public HashCodeCombiner AddEnumerable(params object?[]? enumerable) => AddEnumerable((IEnumerable<object?>?)enumerable);

        public HashCodeCombiner AddEnumerable(IEnumerable<object?>? enumerable)
        {
            if (_coeffNumber >= _coeffs.Length) { throw new Exception("Too many hash code parts!"); }   // TODO : Specify exception type.
            if (enumerable != null)
            {
                unchecked
                {
                    int result = 0;
                    foreach (object? o in enumerable)
                    {
                        if (o != null) { result = result * _coeffs[_coeffNumber] + o.GetHashCode(); }
                    }
                    _hashCode += result;
                }
            }
            _coeffNumber++;
            return this;
        }

        public int Combine() => _hashCode;
    }
}
