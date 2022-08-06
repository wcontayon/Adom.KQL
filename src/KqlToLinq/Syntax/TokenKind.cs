
namespace KqlToLinq.Syntax
{
    /// <summary>
    /// Represents a token type
    /// </summary>
    internal enum TokenKind
    {
        None,
        OpenParenthesis,
        CloseParenthesis,
        EqualOperator,
        NotEqualOperator,
        ContainsOperator,
        GreatherThanOperator,
        GreaterThanOrEqualOperator,
        LessThanOrEqualOperator,
        LessThanOperator,
        QueryOperation,
        QueryColumnName,
        OrOperand,
        AndOperand,
        StringValue,
        NumberValue,
        DateTimeValue,
        BoolValue,
        WhiteSpace,
        StartToken,
    }
}
