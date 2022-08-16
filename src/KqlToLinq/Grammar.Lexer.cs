
using System.Text;

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
        private readonly TokenRule _whiteSpaceTokeRule;
        private readonly IReadOnlyCollection<TokenRule> _singleCharTokenRules;
        private readonly IReadOnlyCollection<TokenRule> _operatorTokenRules;
        private readonly TokenRule _andTokenRule, _orTokenRule;
        private readonly TokenRule _constValueNumberTokenRule;
        private readonly TokenRule _constValueStringTokenRule;
        private readonly TokenRule _constValueDateTimeTokenRule;
        private readonly TokenRule _colNameTokenRules;
        private readonly char[] _specialChararters = new char[] { '*', ' ', ',', '-', '/', '\\', '_', '^', '\'', '~', ':', ';', '(', ')', '{', '}', '#' };
        private readonly char[] _invalidColumnChars = new char[] { '-', '+', '\"' };

        // comparison token rule
        private readonly TokenRule _equalsTokenRule;
        private readonly TokenRule _notEqualTokenRule;
        private readonly TokenRule _greatherThanTokenRule;
        private readonly TokenRule _greaterThanOrEqualTokenRule;
        private readonly TokenRule _lessThanTokenRule;
        private readonly TokenRule _lessThanOrEqualTokenRule;
       
        internal Lexer(IReadOnlyCollection<TokenRule> rules)
        {
            _rules = rules;

            // define specifics token rules, used in the tokenizer
            _whiteSpaceTokeRule = _rules.First(r => r.Token == Syntax.TokenKind.WhiteSpace);
            _colNameTokenRules = _rules.First(r => r.Token == Syntax.TokenKind.QueryColumnName);
            _constValueStringTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.StringValue);
            _constValueNumberTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.NumberValue);
            _constValueDateTimeTokenRule = _rules.First(r => r.Token==Syntax.TokenKind.DateTimeValue);
            _singleCharTokenRules = _rules.Where(r => r.Token == Syntax.TokenKind.OpenParenthesis ||
                                                      r.Token == Syntax.TokenKind.CloseParenthesis).ToList();

            _operatorTokenRules = _rules.Where(r => r.Token == Syntax.TokenKind.EqualOperator ||
                                                      r.Token == Syntax.TokenKind.NotEqualOperator ||
                                                      r.Token == Syntax.TokenKind.GreaterThanOrEqualOperator ||
                                                      r.Token == Syntax.TokenKind.GreatherThanOperator ||
                                                      r.Token == Syntax.TokenKind.LessThanOperator ||
                                                      r.Token == Syntax.TokenKind.LessThanOrEqualOperator ||
                                                      r.Token == Syntax.TokenKind.ContainsOperator).ToList();

            _andTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.AndOperand);
            _orTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.OrOperand);

            _equalsTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.EqualOperator);
            _notEqualTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.NotEqualOperator);
            _greatherThanTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.GreatherThanOperator);
            _greaterThanOrEqualTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.GreaterThanOrEqualOperator);
            _lessThanTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.LessThanOperator);
            _lessThanOrEqualTokenRule = _rules.First(r => r.Token == Syntax.TokenKind.LessThanOrEqualOperator);
        }

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
            var stackParenthesis = new Stack<Token>();

            if (input.IsEmpty)
            {
                return tokens;
            }

            int index = 0;
            int countParenthesis = 0;
            int step = index;

            // we start by processing the first char
            while (index < input.Length)
            {
                var textOrChar = input[index];
                Token? token;

                // if the char is a white space,
                // we move to the next char
                if (_whiteSpaceTokeRule.TryMatch(textOrChar.ToString(), out token))
                {
                    index++;
                    continue;
                }

                // If the char or string is an comparison operator
                // We check if we are not on the last character
                if (IsAnComparisonOperator(input.Slice(index, input.Length - (index + 1) >= 2 ? 2 : (input.Length - (index + 1))), index, out token))
                {
                    tokens.Enqueue(token!, index);
                    IncrementIndex(token!.Text!.Length, ref index);
                    continue;
                }

                // if the char is a single char between (,)
                foreach (var rule in _singleCharTokenRules)
                {
                    if (rule.TryMatch(textOrChar.ToString(), out token))
                    {
                        tokens.Enqueue(token!, index);

                        // Before we continue, we check if we have an (, and is closure )
                        if (token!.Kind == Syntax.TokenKind.OpenParenthesis)
                        {
                            countParenthesis++;
                            stackParenthesis.Push(token!);
                        }
                        else if (token.Kind == Syntax.TokenKind.CloseParenthesis)
                        {
                            countParenthesis--;
                            stackParenthesis.Pop();
                        }

                        IncrementIndex(1, ref index);
                        break;
                    }
                }

                if (token == null)
                {
                    // we continue the parsing, maybe we meet a word
                    string word = textOrChar.ToString();

                    // we iterate througth the input until we find the next special character
                    // or any single token chararter :(,),=,<,> or whitespace
                    
                    if (textOrChar == '\'')
                    {
                        // if the current character is  a quote "'" we have a string constant value
                        // we move to the next quote character to get the value
                        // Start from the next index
                        word = input.Slice(index + 1, MoveToNextQuote(input, index + 1)).ToString();

                        // last check: Is it a datetime value 'yyyy-mm-dd'
                        if (_constValueDateTimeTokenRule.TryMatch(word, out token))
                        {
                            tokens.Enqueue(token!, token!.Position);

                            // Increment the current index => (index+1 + (lenght + 1))
                            IncrementIndex(word.Length + 2, ref index);
                            continue;
                        }

                        // Create the token
                        tokens.Enqueue(new Token(Syntax.TokenKind.StringValue, word.AsSpan(), index+1), index+1);

                        // Move to next char by escaping the end quote
                        IncrementIndex(word.Length + 2, ref index);
                        continue;
                    }
                    else
                    {
                        // Maybe we have a number value, or columnName, or any other operator
                        // We move to the next special char or whitespace
                        word = input.Slice(index, MoveToNextSpecialCharacter(input, index)).ToString();

                        // Is it an operand key (and / or)
                        if (_andTokenRule.TryMatch(word, out token) ||
                            _orTokenRule.TryMatch(word, out token))
                        {
                            tokens.Enqueue(token!, index);

                            IncrementIndex(word.Length, ref index);
                            continue;
                        }

                        // Is it a number const value
                        if (_constValueNumberTokenRule.TryMatch(word, out token))
                        {
                            tokens.Enqueue(token!, index);

                            IncrementIndex(word.Length, ref index);
                            continue;
                        }

                        // Is it a bool const value
                        if ((word.Length == 1 && (word == "1" || word == "0")) ||
                            (word.ToLower() == "true" || word.ToLower() == "false"))
                        {
                            tokens.Enqueue(new Token(Syntax.TokenKind.BoolValue, word.AsSpan(), index), index);

                            IncrementIndex(word.Length, ref index);
                            continue;
                        }

                        // Is it a column name
                        // we validate the column name
                        if (_colNameTokenRules.TryMatch(word, out token))
                        {
                            // We should not have a invalid charater in the column name
                            if (word.Any(c => _invalidColumnChars.Contains(c)))
                            {
                                ThrowHelpers.InvalidColumnNameException(token!.Text!, token!.Position);
                            }

                            tokens.Enqueue(token!, index);

                            IncrementIndex(word.Length, ref index);
                            continue;
                        }
                    }
                }
            }

            // Increment safetly the index
            void IncrementIndex(int step, ref int index)
            {
                if (index < 0)
                {
                    index = 0;
                }
                index += step;
            }

            // Move the span to the next "'" character to 
            // build a word
            int MoveToNextQuote(ReadOnlySpan<char> input, int startIndex)
            {
                int length = 0;
                for (int i = startIndex; i < input.Length; i++)
                {
                    if (input[i] == '\'')
                    {
                        break;
                    }

                    length++;
                }

                return length;
            }

            // Move the span to the next special character to 
            // build a word
            int MoveToNextSpecialCharacter(ReadOnlySpan<char> input, int startIndex)
            {
                int length = 0;
                for (int i = startIndex; i < input.Length; i++)
                {
                    if (_specialChararters.Contains(input[i]))
                    {
                        break;
                    }

                    length++;
                }

                return length;
            }
            

            // check that we do not have a bad input (parenthesis are correctly closde)
            if (countParenthesis > 0)
            {
                // Throw the exception with the first OpenParenthesis found
                var openParenthesis = stackParenthesis.Pop();
                ThrowHelpers.OpenParenthesisNotClosedException(openParenthesis.Text!, openParenthesis.Position);
            }

            return tokens;
        }


        private bool IsAnComparisonOperator(ReadOnlySpan<char> input, int startIndex, out Token? token)
        {
            if (input.IsEmpty)
            {
                token = null;
                return false;
            }

            var firstChar = input[0];
            
            if (firstChar == '>' || firstChar == '!' || firstChar == '<')
            {
                // Let's see if the second char is '='
                var possibleWord = input.Slice(0, 2);
                if (_lessThanOrEqualTokenRule.TryMatch(possibleWord.ToString(), out token) ||
                    _greaterThanOrEqualTokenRule.TryMatch(possibleWord.ToString(), out token) ||
                    _notEqualTokenRule.TryMatch(possibleWord.ToString(), out token))
                {
                    return true;
                }
                else if (_lessThanTokenRule.TryMatch(input.ToString(), out token) ||
                         _greatherThanTokenRule.TryMatch(input.ToString(), out token))
                {
                    return true;
                }
            }
            else if (_equalsTokenRule.TryMatch(input.ToString(), out token))
            {
                return true;
            }

            token = null;

            return false;
        }
    }
}
