using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KqlToLinq.Syntax
{
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
        QueryTypeValue,
        WhiteSpace,
        StartToken,
    }
}
