
namespace KqlToLinq;

internal partial class Grammar
{
    /// <summary>
    /// Internal Lexer. This lexer is used to tokenize the
    /// input string.
    /// </summary>
	internal class Lexer
    {
        private readonly IReadOnlyCollection<TokenRule> _rules;

        internal Lexer(IReadOnlyCollection<TokenRule> rules) => _rules = rules;

        /// <summary>
        /// Parse the input <see cref="ReadOnlySpan{char}"/> into
        /// a <see cref="Stack{Token}"/>.
        /// </summary>
        /// <param name="input">The input to parse. <see cref="ReadOnlySpan{char}"/></param>
        /// <returns><see cref="PriorityQueue{Token}"/></returns>
        internal PriorityQueue<Token, int> Tokenize(ReadOnlySpan<char> input)
        {
            // We use a PriorityQueue, in order to process in min order the token
            // starting by the first token (postion 0) to the last (position = length)
            PriorityQueue<Token, int> tokens = new PriorityQueue<Token, int>();
            var matches = new SortedList<int, Token>();
            
            foreach (var rule in _rules)
            {
                // for each rules, we get all the matches occurrences
                var tokensFound = rule.IsMatch(input);
                if (tokensFound.Count() > 0)
                {
                    tokens.EnqueueRange(tokensFound.Select(t => (t, t.Position)));
                }
            }

            return tokens;
        }
    }
}
