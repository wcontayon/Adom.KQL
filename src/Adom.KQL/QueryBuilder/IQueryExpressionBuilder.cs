using Adom.KQL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Adom.KQL.Tests")]
namespace Adom.KQL.QueryBuilder;

internal interface IQueryExpressionBuilder
{
    /// <summary>
    /// Visit a <see cref="QuerySyntax"/> syntax to build
    /// an <see cref="Expression"/>
    /// </summary>
    /// <param name="syntax"><see cref="QuerySyntax"/></param>
    /// <returns><see cref="Expression"/></returns>
    Expression EvaluateQuerySyntax(QuerySyntax syntax);

    /// <summary>
    /// Visit a <see cref="QueryOperandKind"/> to build an
    /// <see cref="Expression"/>.
    /// </summary>
    /// <param name="operandKind"><see cref="QueryOperandKind"/></param>
    /// <returns><see cref="Expression"/></returns>
    Expression EvaluateBinaryOperand(QueryOperandKind operandKind, Expression left, Expression right);
}
