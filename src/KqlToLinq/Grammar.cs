using KqlToLinq.Syntax;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("KqlToLinq.Tests")]
namespace KqlToLinq;

internal partial class Grammar
{
    [ThreadStatic]
    private readonly IReadOnlyCollection<TokenRule> rules;
    private static readonly object _lock = new object();
    
    // For test
    internal static Grammar? _grammar;
    internal readonly Lexer _lexer;

    public Grammar()
    {
        rules = new List<TokenRule>()
        {
            new TokenRule(TokenKind.AndOperand, "^and"),
            new TokenRule(TokenKind.OrOperand, "^or"),
            new TokenRule(TokenKind.OpenParenthesis, "\\("),
            new TokenRule(TokenKind.CloseParenthesis, "\\)"),
            new TokenRule(TokenKind.StringValue, "'([^']*)'"),
            new TokenRule(TokenKind.NumberValue, "\\d+"),
            new TokenRule(TokenKind.DateTimeValue, "'(\\d{4})-(\\d{2})-(\\d{2})( (\\d{2}):(\\d{2}):(\\d{2}))?'"),
            new TokenRule(TokenKind.EqualOperator, "="),
            new TokenRule(TokenKind.NotEqualOperator, "!="),
            new TokenRule(TokenKind.GreaterThanOrEqualOperator, ">="),
            new TokenRule(TokenKind.GreatherThanOperator, ">"),
            new TokenRule(TokenKind.LessThanOrEqualOperator, "<="),
            new TokenRule(TokenKind.LessThanOperator, "<"),
            new TokenRule(TokenKind.NotEqualOperator, "!="),
            new TokenRule(TokenKind.QueryColumnName, "/^\\w+/"),
            new TokenRule(TokenKind.WhiteSpace, "\\s+")
        };

        _lexer = new Lexer(rules);
    }

    public static Grammar Instance()
    {
        if (_grammar == null)
        {
            lock (_lock)
            {
                if (_grammar == null)
                {
                    _grammar = new Grammar();
                }
            }
        }

        return _grammar;
    }

    /// <summary>
    /// Represents a rule to match a <see cref="Token"/>
    /// </summary>
    internal class TokenRule
    {
        private readonly string _pattern;

        public TokenKind Token { get; set; }

        public TokenRule(TokenKind kind, string pattern)
        {
            Token = kind;
            _pattern = pattern;
        }

        /// <summary>
        /// Searches all occurrences of the current <see cref="TokenKind"/> inside 
        /// the <see cref="ReadOnlySpan{char}"/> input
        /// </summary>
        /// <param name="input"></param>
        /// <returns><see cref="IEnumerable{Token}"/></returns>
        public IEnumerable<Token> IsMatch(ReadOnlySpan<char> input)
        {
            MatchCollection matches = Regex.Matches(input.ToString(), _pattern, RegexOptions.CultureInvariant);
            if (matches.Count > 0)
            {
                return matches.Select(match => new Token(Token, match.ValueSpan, match.Index));
            }

            return Enumerable.Empty<Token>();
        }
    }
}

