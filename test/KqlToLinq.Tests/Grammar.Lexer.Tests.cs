using System;
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
}
