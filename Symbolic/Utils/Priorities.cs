namespace Symbolic.Utils
{
    public enum Priorities : sbyte
    {
        Min                    = -1,
        OuterStandartFunctions = 0,
        InnerNegation          = 1,
        NegativeConstant       = InnerNegation,
        Addition               = 2,
        Multiplication         = 3,
        Division               = 3,
        Exponentiation         = 4,
        OuterNegation          = 5,
        InnerStandartFunctions = 6,
        Constant               = 6,
        NonNegativeConstant    = Constant,
        Symbol                 = 6,
        Max                    = 100
    }
}
