
namespace KqlToLinq.Syntax;

/// <summary>
/// Represents a word, phrase, operand or a query syntax
/// </summary>
internal abstract class Syntax
{
    /// <summary>
    /// The <see cref="SyntaxKind"/> of the syntax
    /// </summary>
    public abstract SyntaxKind Kind { get; }

    /// <summary>
    /// The text of the <see cref="Syntax"/>
    /// </summary>
    public abstract SyntaxText Text { get; }
 
    /// <summary>
    /// The <see cref="Token"/> array inside the syntax.
    /// The syntax can have one, or multiple tokens.
    /// </summary>
    public abstract Token[] Tokens { get; }
}