using Adom.KQL.Syntax;
using System.Linq.Expressions;

namespace Adom.KQL;

/// <summary>
/// Visitor interface used to build the query expression
/// on the SyntaxTree
/// </summary>
internal interface INodeVisitor
{
    /// <summary>
    /// Visit a <see cref="QuerySyntax"/> syntax to build
    /// an <see cref="Expression"/>
    /// </summary>
    /// <param name="syntax"><see cref="QuerySyntax"/></param>
    /// <returns><see cref="Expression"/></returns>
    Expression VisiteQuerySyntax(QuerySyntax syntax);

    /// <summary>
    /// Visit a <see cref="QueryOperandKind"/> to build an
    /// <see cref="Expression"/>.
    /// </summary>
    /// <param name="operandKind"><see cref="QueryOperandKind"/></param>
    /// <returns><see cref="Expression"/></returns>
    Expression VisiteBinaryOperand(QueryOperandKind operandKind);
}