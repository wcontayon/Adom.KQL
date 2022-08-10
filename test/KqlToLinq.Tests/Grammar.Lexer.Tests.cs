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

	[Trait("Category", "Lexer")]
	[Theory(DisplayName = "Grammar/Lexer should tokenize an input string")]
	//[InlineData("col = 'value' and dateCol = '2022-08-01'", 7)]
	//[InlineData("col = 'value'", 3)]
	[InlineData("colNumb > 37 or colNumb <= 50", 7)]
	//[InlineData("(colNum != 7 and colDate = '2022-08-01') or (col != 'value')", 15)]
	//[InlineData("(col = 'value1' or col = 'value2') and dateCol = '2022-08-01'", 13)]
	
	//[InlineData("((col1 = 'value' and col2 = 'valueCol2') or (col3 = 7)) and dateCol = '2022-08-01'", 21)]
	public void GrammarLexerShouldTokenizeNonEmptyPriorityQueue(string inputText, int nbToken)
    {
		// Arrange
		var grammar = Grammar.Instance();

		// Act
		var tokensQueue = grammar._lexer.Tokenize(inputText.AsSpan());

		// Assert
		Assert.Equal(nbToken, tokensQueue.Count);
    }
}
