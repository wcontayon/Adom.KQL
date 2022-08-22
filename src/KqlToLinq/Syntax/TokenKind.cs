
namespace KqlToLinq.Syntax
{
    /// <summary>
    /// Represents a token type
    /// </summary>
    public enum TokenKind
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

    public static class TokenKinds
    {
        public static bool IsLeftToken(this TokenKind tokenKind) => tokenKind == TokenKind.QueryColumnName;

        public static bool IsRightToken(this TokenKind tokenKind) => tokenKind == TokenKind.DateTimeValue ||
                                                          tokenKind == TokenKind.NumberValue ||
                                                          tokenKind == TokenKind.StringValue ||
                                                          tokenKind == TokenKind.BoolValue;

        public static bool IsOperator(this TokenKind tokenKind) =>  tokenKind == TokenKind.EqualOperator ||
                                                                    tokenKind == TokenKind.NotEqualOperator ||
                                                                    tokenKind == TokenKind.ContainsOperator ||
                                                                    tokenKind == TokenKind.GreatherThanOperator ||
                                                                    tokenKind == TokenKind.GreaterThanOrEqualOperator ||
                                                                    tokenKind == TokenKind.LessThanOrEqualOperator ||
                                                                    tokenKind == TokenKind.LessThanOperator;

        public static bool IsBinaryOperand(this TokenKind tokenKind) => tokenKind == TokenKind.OrOperand ||
                                                                  tokenKind == TokenKind.AndOperand;
    }
}
