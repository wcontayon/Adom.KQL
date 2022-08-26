
namespace Adom.KQL.Syntax
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
        /// <summary>
        /// Check if the token is a <see cref="TokenKind.QueryColumnName"/>
        /// </summary>
        /// <param name="tokenKind"><see cref="TokenKind"/></param>
        /// <returns><see cref="true" /> or <see cref="false" /></returns>
        public static bool IsLeftToken(this TokenKind tokenKind) => tokenKind == TokenKind.QueryColumnName;

        /// <summary>
        /// Check if the token is a constant value"/>
        /// </summary>
        /// <param name="tokenKind"><see cref="TokenKind"/></param>
        /// <returns><see cref="true" /> or <see cref="false" /></returns>
        public static bool IsRightToken(this TokenKind tokenKind) => tokenKind == TokenKind.DateTimeValue ||
                                                          tokenKind == TokenKind.NumberValue ||
                                                          tokenKind == TokenKind.StringValue ||
                                                          tokenKind == TokenKind.BoolValue;

        /// <summary>
        /// Check if the token is a condition operator (=, !=, >, >=, <, <=)"/>
        /// </summary>
        /// <param name="tokenKind"><see cref="TokenKind"/></param>
        /// <returns><see cref="true" /> or <see cref="false" /></returns>
        public static bool IsOperator(this TokenKind tokenKind) =>  tokenKind == TokenKind.EqualOperator ||
                                                                    tokenKind == TokenKind.NotEqualOperator ||
                                                                    tokenKind == TokenKind.ContainsOperator ||
                                                                    tokenKind == TokenKind.GreatherThanOperator ||
                                                                    tokenKind == TokenKind.GreaterThanOrEqualOperator ||
                                                                    tokenKind == TokenKind.LessThanOrEqualOperator ||
                                                                    tokenKind == TokenKind.LessThanOperator;

        /// <summary>
        /// Check if the token is an binary operand (and, or)"/>
        /// </summary>
        /// <param name="tokenKind"><see cref="TokenKind"/></param>
        /// <returns><see cref="true" /> or <see cref="false" /></returns>
        public static bool IsBinaryOperand(this TokenKind tokenKind) => tokenKind == TokenKind.OrOperand ||
                                                                  tokenKind == TokenKind.AndOperand;
    }
}
