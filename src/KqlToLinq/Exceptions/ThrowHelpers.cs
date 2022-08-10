using KqlToLinq.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KqlToLinq;

internal class ThrowHelpers
{
    [DoesNotReturn]
    internal static void OpenParenthesisNotClosedException(string token, int position) 
        => throw new BadExpressionInputFormatException(token, position, ExceptionMessages.PARENTHESIS_NON_CLOSE);

    [DoesNotReturn]
    internal static void InvalidColumnNameException(string token, int position)
        => throw new BadExpressionInputFormatException(token, position, ExceptionMessages.INVALID_FIELDNAME);
}
