// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

using Adom.KQL.Syntax;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Adom.KQL.Tests")]
namespace Adom.KQL;

[DebuggerDisplay("TokenKind = {Kind}, Text = {Text}")]
public class Token : IEquatable<Token>
{
    private readonly TokenKind _kind;
    private readonly string? _text;
    private readonly int _position;
    private readonly int _wordLength;

    public Token(TokenKind kind, Span<char> word, int position)
    {
        ArgumentNullException.ThrowIfNull(kind, nameof(kind));

        _kind = kind;
        _text = word.ToString();
        _position = position;
        _wordLength = word.Length;
    }

    public Token(TokenKind kind, ReadOnlySpan<char> word, int position)
    {
        ArgumentNullException.ThrowIfNull(kind, nameof(kind));

        _kind = kind;
        _text = word.ToString();
        _position = position;
        _wordLength = word.Length;
    }

    public TokenKind Kind => _kind;

    public string? Text => _text;

    public int Position => _position;

    public int WordLength => _wordLength;

    public bool Equals(Token? other) => other != null && 
                                        other.Kind == _kind &&
                                        other.Text == _text &&
                                        other.Position == _position &&
                                        other.WordLength == _wordLength;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is Token token) return Equals(token);
        return false;
    }

    public override int GetHashCode() => _text.GetHashCode(StringComparison.InvariantCulture) ^
                                        _position.GetHashCode() ^
                                        _wordLength.GetHashCode();
}
