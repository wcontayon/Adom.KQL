using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KqlToLinq.Syntax;

/// <summary>
/// Represents a type of syntax
/// </summary>
internal enum SyntaxKind
{
    None,
    QuerySyntax,
    OrOperand,
    AndOperand,
    OpenParenthesis,
    CloseParenthesis,
    ColumnName,
    ConstValue,
    Operator
}
