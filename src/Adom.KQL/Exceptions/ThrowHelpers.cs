// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

using Adom.KQL.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Adom.KQL;

internal static class ThrowHelpers
{
    [DoesNotReturn]
    internal static void OpenParenthesisNotClosedException(string token, int position) 
        => throw new BadExpressionInputFormatException(token, position, ExceptionMessages.PARENTHESIS_NON_CLOSE);

    [DoesNotReturn]
    internal static void ClosedParenthesisWithoutAnOpendException(string token, int postion)
        => throw new BadExpressionInputFormatException(token, postion, ExceptionMessages.CLOSED_PARENTHESIS_WITHOUT_THIS_OPENED);

    [DoesNotReturn]
    internal static void InvalidColumnNameException(string token, int position)
        => throw new BadExpressionInputFormatException(token, position, ExceptionMessages.INVALID_FIELDNAME);

    [DoesNotReturn]
    internal static void IncorrectQuerySyntax(string text)
        => throw new QuerySyntaxEpressionException(text, ExceptionMessages.QUERY_SYNTAX_INCORRECT);

    [DoesNotReturn]
    internal static void UnknownFieldInQuery(string fieldName, string typeName)
        => throw new UnknownFieldException(fieldName, typeName, string.Format(CultureInfo.InvariantCulture,ExceptionMessages.UNKNOWN_FIELDNAME_IN_QUERY, fieldName, typeName));

    [DoesNotReturn]
    internal static void UnknownOperator(string @operator)
        => throw new UnknownOperatorException(@operator, string.Format(CultureInfo.InvariantCulture, ExceptionMessages.UNKNOW_OPERATOR, @operator));
}
