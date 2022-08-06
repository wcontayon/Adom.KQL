
using KqlToLinq.Syntax;

namespace KqlToLinq;

internal class Keyword
{

    public static (char Token, string Pattern) OpenParenthesis = ('(', "^(");
    public static (char Token, string Pattern) CloseParenthesis = (')', "^)");
    public static (char Token, string Pattern) CloseBracket = (']', "^]");
    public static (char Token, string Pattern) OpenBracket = ('[', "^[");
    public static (char Token, string Pattern) EqualsOperator = ('=', "^=");
    public static (string Token, string Pattern) NotEquals = ("!=", "^!=");
    public static (char Token, string Pattern) LessThan = ('<', "^<");
    public static (string Token, string Pattern) LessThanOrEqual = ("<=", "^<=");
    public static (char Token, string Pattern) GreaterThan = ('>', "^>");
    public static (string Token, string Pattern) GreaterThanOrEqual = (">=", "^>=");
    public static (char Token, string Pattern) Star = ('*', "^*");
    public static (char Token, string Pattern) ContainsDoublePoint = (':', "^:");
    public static (string Token, string Pattern) OrOperand = ("or", "^or");
    public static (string Token, string Pattern) AndOperand = ("and", "^and");
    public static (char Token, string Pattern) WhiteSpace = (' ', "^ ");
}
