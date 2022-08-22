using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KqlToLinq.QueryBuilder;
using KqlToLinq.Syntax;
using Xunit;

namespace KqlToLinq.Tests;

public class GrammarParserTests
{
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

	class TestClass
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

    // [Trait("Category", "Parser")]
    // [Theory(DisplayName = "Grammar/Parser should parse and evaluate correctly an input")]
    // public void GrammarParserShouldEvaluateCorrectly()
    // {

    // }
}
