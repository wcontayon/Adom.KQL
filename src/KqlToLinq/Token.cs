using KqlToLinq.Syntax;

namespace KqlToLinq;

internal class Token
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
}
