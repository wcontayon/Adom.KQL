using System;
using System.Linq.Expressions;
using KqlToLinq.QueryBuilder;
using KqlToLinq.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace KqlToLinq.Tests;

public class GrammarParserTests
{
    private readonly ITestOutputHelper _output;

    public GrammarParserTests(ITestOutputHelper output) => _output = output;

    [Trait("Category", "Parser")]
	[Fact(DisplayName = "Grammar/Parser should be unique")]
	public void GrammerParserShouldBeUnique()
    {
		// Act
		var grammar1 = Grammar.Instance();
		var grammar2 = Grammar.Instance();

		// Assert
		Assert.True(grammar1 == grammar2);
		Assert.True(grammar2 == grammar1);
		Assert.True(grammar1._parser == grammar2._parser);
		Assert.True(grammar2._parser == grammar1._parser);
	}

	public class TestClass
    {
        public string? StringField { get; set; }

        public int NumberField { get; set; }

        public DateTime DateTimeField { get; set; }

        public DateOnly DateOnlyField { get; set; }
    }

    class TestQueryBuilder<T> : IQueryExpressionBuilder
    {
        public int CountEvaluateBinary { get; set; }

        public int CountEvaluateQuerySyntax { get; set; }

        public Expression EvaluateBinaryOperand(QueryOperandKind operandKind, Expression left, Expression right)
        {
            CountEvaluateBinary++;
            return Expression.Empty();
        }

        public Expression EvaluateQuerySyntax(QuerySyntax syntax)
        {
            CountEvaluateQuerySyntax++;
            return Expression.Empty();
        }
    }

    [Trait("Category", "Parser")]
    [Theory(DisplayName = "Grammar/Parser should parse and evaluate correctly an input")]
    [InlineData("(StringField = 'value')", 1, 0)]
    [InlineData("StringField = 'value' or StringField = 'value2'", 2, 1)]
    [InlineData("((StringField = 'value' or StringField = 'value2') and NumberField = 10 and DateTimeField = '2022-08-01')", 4, 3)]
    [InlineData("((StringField = 'value' or StringField = 'value2') and NumberField = 10) and DateTimeField = '2022-08-01'", 4, 3)]
    [InlineData("StringField = 'value' or StringField = 'value2' and NumberField = 10 and (DateTimeField = '2022-08-01' or DateOnlyField = '2022-08-01')", 5, 4)]
    public void GrammarParserShouldEvaluateCorrectly(string input, int querySyntaxCount, int binaryOperandCount)
    {
        // Arrange
        var grammar = Grammar.Instance();
        var testQueryBuilder = new TestQueryBuilder<TestClass>();

        // Act
        var tokens = grammar._lexer.Tokenize(input);
        var expression = grammar._parser.Parse(tokens, testQueryBuilder);

        // Assert
        Assert.True(testQueryBuilder.CountEvaluateBinary == binaryOperandCount);
        Assert.True(testQueryBuilder.CountEvaluateQuerySyntax == querySyntaxCount);
    }
}
