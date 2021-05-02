using System;

namespace Symbolic.Utils
{
    public static class Properties
    {
        public static bool UseCaching
        {
            get => _useCaching;
            set
            {
                if (!value) { FunctionFactory.ClearCache(); }
                _useCaching = value;
            }
        }

        private static bool _useCaching = true;

        public class WithoutCaching : IDisposable
        {
            public bool UseCacheAfterDisposing { get; set; }

            public WithoutCaching()
            {
                UseCacheAfterDisposing = _useCaching;
                _useCaching = false;
            }

            public void Dispose() => _useCaching = UseCacheAfterDisposing;
        }
    }
}
