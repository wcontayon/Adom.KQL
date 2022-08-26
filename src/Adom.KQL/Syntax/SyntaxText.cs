
using System.Text;

namespace Adom.KQL.Syntax;

/// <summary>
/// Represents the text of a <see cref="Syntax"/>
/// </summary>
internal readonly struct SyntaxText
{
    private readonly string _text;
    private readonly int _length;
    private readonly int _startPosition;

    public SyntaxText(Span<Token> tokens)
    {
        // The text start a the first position
        StringBuilder builder = new StringBuilder();
        _startPosition = tokens[0].Position;
        for (int i = 0; i < tokens.Length; i++)
        {
            builder.Append(tokens[i].Text);
        }
        _text = builder.ToString();
        _length = builder.Length;
    }

    public string Text => _text;

    public int Length => _length;

    public int StartPosition => _startPosition;
}
