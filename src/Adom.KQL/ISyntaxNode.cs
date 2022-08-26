// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

using System.Linq.Expressions;

namespace Adom.KQL;

/// <summary>
/// Represents a syntax node in the AST (Abstract Syntax Tree)
/// </summary>
internal interface ISyntaxNode
{
    /// <summary>
    /// Accept a <see cref="INodeVisitor"/> to build the query expression
    /// </summary>
    /// <param name="visitor"><see cref="INodeVisitor"/></param>
    /// <returns><see cref="Expression"/></returns>
    Expression Accept(INodeVisitor visitor);
}
