using KqlToLinq.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace KqlToLinq;

public class GrammarLexerTest
{
	[Trait("Category", "Lexer")]
	[Fact(DisplayName = "Grammar/Lexer instance should be unique")]
	public void GrammarLexerShouldBeUnique()
    {
		// Act
		var grammar1 = Grammar.Instance();
		var grammar2 = Grammar.Instance();

		// Assert
		Assert.True(grammar1 == grammar2);
		Assert.True(grammar2 == grammar1);
		Assert.True(grammar1._lexer == grammar2._lexer);
		Assert.True(grammar2._lexer == grammar1._lexer);
    }

	[Trait("Category", "Lexer")]
	[Fact(DisplayName = "Grammar/Lexer should tokenize an empty PriorityQueue")]
	public void GrammarLexerShouldTokenizeAnEmptyPriorityQueue()
    {
		// Arrange
		string input = "";
		var grammar = Grammar.Instance();

		// Act
		var tokens = grammar._lexer.Tokenize(input.AsSpan());

		// Assert
		Assert.True(tokens.Count == 0);
    }

	[Trait("Category", "Lexer")]
	[Theory(DisplayName = "Grammar/Lexer should tokenize an input string")]
    [InlineData("col = 'value' and dateCol = '2022-08-01'", 7)]
    [InlineData("col = 'value'", 3)]
    [InlineData("colNumb > 37 or colNumb <= 50", 7)]
    [InlineData("(colNum != 7 and colDate = '2022-08-01') or (col != 'value')", 15)]
    [InlineData("(col = 'value1' or col = 'value2') and dateCol = '2022-08-01'", 13)]
    [InlineData("((col1 = 'value' and col2 = 'valueCol2') or (col3 = 7)) and dateCol = '2022-08-01'", 21)]
    public void GrammarLexerShouldTokenizeNonEmptyPriorityQueue(string inputText, int nbToken)
    {
		// Arrange
		var grammar = Grammar.Instance();

		// Act
		var tokensQueue = grammar._lexer.Tokenize(inputText.AsSpan());

		// Assert
		Assert.Equal(nbToken, tokensQueue.Count);
    }

	public class LexerDataTest
    {
        public string? InputText { get; set; }

        public List<(Token, int)>? Tokens { get; set; }
    }

	internal class TokensDataGenerator : IEnumerable<object[]>
    {
		public static IEnumerable<object[]> GenerateTokens()
        {
			yield return new object[]
			{
				new LexerDataTest()
				{
					InputText = "col = 'value' and dateCol = '2022-08-01'",
					Tokens = new List<(Token, int)>
					{
						ValueTuple.Create(new Token(Syntax.TokenKind.QueryColumnName, "col".AsSpan(), 0), 0),
						ValueTuple.Create(new Token(Syntax.TokenKind.EqualOperator, "=".AsSpan(), 4), 4),
						ValueTuple.Create(new Token(Syntax.TokenKind.StringValue, "value".AsSpan(), 7), 7),
						ValueTuple.Create(new Token(Syntax.TokenKind.AndOperand, "and".AsSpan(), 14), 14),
						ValueTuple.Create(new Token(Syntax.TokenKind.QueryColumnName, "dateCol".AsSpan(), 18), 18),
						ValueTuple.Create(new Token(Syntax.TokenKind.EqualOperator, "=".AsSpan(), 26), 26),
						ValueTuple.Create(new Token(Syntax.TokenKind.DateTimeValue, "2022-08-01".AsSpan(), 29), 29)
					}
				}
			};

			yield return new object[]
			{
				new LexerDataTest()
				{
					InputText = "col = 'value'",
					Tokens = new List<(Token, int)>
					{
						ValueTuple.Create(new Token(Syntax.TokenKind.QueryColumnName, "col".AsSpan(), 0), 0),
						ValueTuple.Create(new Token(Syntax.TokenKind.EqualOperator, "=".AsSpan(), 4), 4),
						ValueTuple.Create(new Token(Syntax.TokenKind.StringValue, "value".AsSpan(), 7), 7),
					}
				}
			};

			yield return new object[]
			{
				new LexerDataTest()
				{
					InputText = "colNumb > 37 or colNumb <= 50",
					Tokens = new List<(Token, int)>
					{
						ValueTuple.Create(new Token(Syntax.TokenKind.QueryColumnName, "colNumb".AsSpan(), 0), 0),
						ValueTuple.Create(new Token(Syntax.TokenKind.GreatherThanOperator, ">".AsSpan(), 8), 8),
						ValueTuple.Create(new Token(Syntax.TokenKind.NumberValue, "37".AsSpan(), 10), 10),
						ValueTuple.Create(new Token(Syntax.TokenKind.OrOperand, "or".AsSpan(), 13), 13),
						ValueTuple.Create(new Token(Syntax.TokenKind.QueryColumnName, "colNumb".AsSpan(), 16), 16),
						ValueTuple.Create(new Token(Syntax.TokenKind.LessThanOrEqualOperator, "<=".AsSpan(), 24), 24),
						ValueTuple.Create(new Token(Syntax.TokenKind.DateTimeValue, "50".AsSpan(), 27), 27)
					}
				}
			};
        }

		public IEnumerator<object[]> GetEnumerator() => GenerateTokens().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GenerateTokens().GetEnumerator();
    }

    [Trait("Category", "Lexer")]
	[Theory(DisplayName = "Grammar/Lexer should tokenize correct tokens")]
	[MemberData(nameof(TokensDataGenerator.GenerateTokens), MemberType = typeof(TokensDataGenerator))]
    public void GrammarLexerShouldTokenizeCorrectTokens(LexerDataTest inputLexer)
    {
		// Arrange
		var grammar = Grammar.Instance();

		// Act
		var tokensQueue = grammar._lexer.Tokenize(inputLexer.InputText);

		// Assert
		Assert.NotNull(tokensQueue);
		Assert.Equal(inputLexer.Tokens!.Count, tokensQueue.Count);
		for (var i = 0; i < tokensQueue.Count; i++)
        {
			var token = tokensQueue.Dequeue();
			var expectedToken = inputLexer!.Tokens![i];
			Assert.True(token.Equals(expectedToken.Item1));
        }
    }

	[Trait("Category", "Lexer")]
	[Theory(DisplayName = "Tokenize should raise ParenthesisNotClosed exception")]
	[InlineData("(()")]
	[InlineData("(colNum != 7 and colDate = '2022-08-01') or (col != 'value'")]
	[InlineData("(col = 'value1' or col = 'value2' and dateCol = '2022-08-01'")]
	[InlineData("((col1 = 'value' and col2 = 'valueCol2' or (col3 = 7)) and dateCol = '2022-08-01'")]
	public void TokenizeShouldRaiseParenthesisNotClosedException(string input)
    {
		// Arrange
		var grammar = Grammar.Instance();

		// Act
		var exception = Assert.Throws<BadExpressionInputFormatException>(() => grammar._lexer.Tokenize(input));
		Assert.True(exception.Message == ExceptionMessages.PARENTHESIS_NON_CLOSE);
    }

	[Trait("Category", "Lexer")]
	[Theory(DisplayName = "Tokenise should raise an exception if ')' is found without '('")]
	[InlineData(")")]
	[InlineData("())")]
	[InlineData("))")]
	[InlineData("(((()))))")]
	public void TokenizeShouldRaiseExceptionIfCloseParenthesisFoundWithoutHisOpend(string input)
	{
		// Arrange
		var grammar = Grammar.Instance();

		// Act and Assert
		var exception = Assert.Throws<BadExpressionInputFormatException>(() => grammar._lexer.Tokenize(input));
		Assert.True(exception.Message == ExceptionMessages.CLOSED_PARENTHESIS_WITHOUT_THIS_OPENED);
	}
}

