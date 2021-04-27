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
    }
}
