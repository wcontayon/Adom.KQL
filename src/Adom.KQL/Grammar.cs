// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

using Adom.KQL.Syntax;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Adom.KQL.Tests")]
namespace Adom.KQL;

internal partial class Grammar
{
    [ThreadStatic]
    private readonly IReadOnlyCollection<TokenRule> rules;
    private static readonly object _lock = new object();
    
    // For test
    internal static Grammar? _grammar;
    internal readonly Lexer _lexer;
    internal readonly Parser _parser;

    public Grammar()
    {
        rules = new List<TokenRule>()
        {
            new TokenRule(TokenKind.AndOperand, "^and"),
            new TokenRule(TokenKind.OrOperand, "^or"),
            new TokenRule(TokenKind.OpenParenthesis, @"\("),
            new TokenRule(TokenKind.CloseParenthesis, @"\)"),
            new TokenRule(TokenKind.StringValue, "'([^']*)'"),
            new TokenRule(TokenKind.NumberValue, "^-?(?:(?:0|[1-9][0-9]*)(?:,[0-9]+)?|[1-9][0-9]{1,2}(?:,[0-9]{3})+)$"),
            new TokenRule(TokenKind.DateTimeValue, @"^(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$"),
            new TokenRule(TokenKind.EqualOperator, "="),
            new TokenRule(TokenKind.GreaterThanOrEqualOperator, ">="),
            new TokenRule(TokenKind.GreatherThanOperator, ">"),
            new TokenRule(TokenKind.LessThanOrEqualOperator, "<="),
            new TokenRule(TokenKind.LessThanOperator, "<"),
            new TokenRule(TokenKind.NotEqualOperator, "!="),
            new TokenRule(TokenKind.QueryColumnName, "(.*?)[a-zA-Z0-9]*"),
            new TokenRule(TokenKind.WhiteSpace, @"\s+")
        };

        

        //Source: https://prograide.com/pregunta/72878/le-lexer-du-pauvre-pour-c

        _lexer = new Lexer(rules);
        _parser = new Parser();
    }

    public static Grammar Instance()
    {
        if (_grammar == null)
        {
            lock (_lock)
            {
#pragma warning disable CA1508 // Éviter le code conditionnel mort
                if (_grammar == null)
                {
                    _grammar = new Grammar();
                }
#pragma warning restore CA1508 // Éviter le code conditionnel mort
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
        /// the <see cref="string"/> input
        /// </summary>
        /// <param name="input"></param>
        /// <returns><see cref="IEnumerable{Token}"/></returns>
        public IEnumerable<Token> IsMatch(string input)
        {
            MatchCollection matches = Regex.Matches(input, _pattern);
            for (int i = 0; i < matches.Count; i++)
            {
                yield return new Token(Token, matches[i].ValueSpan, matches[i].Index);
            }
        }

        /// <summary>
        /// Search the first occurrence of the current <see cref="TokenKind"/>
        /// inside the <see cref="ReadOnlySpan{char}"/> int
        /// </summary>
        /// <param name="input"><see cref="ReadOnlySpan{char}"/></param>
        /// <param name="token"><see cref="Adom.KQL.Token?"/>out param Token</param>
        /// <returns><see cref="(bool IsMatch, Adom.KQL.Token? Token)"/></returns>
        public bool TryMatch(string input, out Token? token)
        {
            var match = Regex.Match(input, _pattern);
            if (match.Success)
            {
                token = new Token(Token, match.ValueSpan, match.Index);
                return true;
            }

            token = null;

            return false;
        }

        /// <summary>
        /// Search the first occurrence of the current <see cref="TokenKind"/>
        /// inside the <see cref="ReadOnlySpan{char}"/> int
        /// </summary>
        /// <param name="input"><see cref="ReadOnlySpan{char}"/></param>
        /// <param name="startIndex">The start index where the word has been found</param>
        /// <param name="token"><see cref="Adom.KQL.Token?"/>out param Token</param>
        /// <returns><see cref="(bool IsMatch, Adom.KQL.Token? Token)"/></returns>
        public bool TryMatch(string input, int startIndex, out Token? token)
        {
            var match = Regex.Match(input, _pattern);
            if (match.Success)
            {
                token = new Token(Token, match.ValueSpan, startIndex);
                return true;
            }

            token = null;

            return false;
        }
    }
}

