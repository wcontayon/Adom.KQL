using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KqlToLinq.Syntax;

internal class QuerySyntax : Syntax
{
    private readonly SyntaxKind _kind;
    // the left token represents the property column name
    private readonly Token _leftToken;
    // the right token represents the value to check for the property column
    private readonly Token _rightToken;
    // the token operator represent the query operator
    private readonly Token _operator;
    // Needed to build the predicate
    private readonly OperatorKind _operatorKind;
    private readonly SyntaxText _text;


    public QuerySyntax(SyntaxKind kind, Span<Token> tokens)
    {
        _kind = kind;
        _text = new SyntaxText(tokens);
    }

    /// <inheritdoc />
    public override SyntaxKind Kind => _kind;

    /// <inheritdoc />
    public override SyntaxText Text => _text;

    public Token LeftToken => _leftToken;
    public Token RightToken => _rightToken;
    public OperatorKind Operator => _operatorKind;

    /// <inheritdoc />
    public override Token[] Tokens => new Token[3] { _leftToken, _operator, _rightToken };
}
