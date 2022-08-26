using Bogus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Globalization;
using Xunit;

namespace Adom.KQL.Collections.Tests;

public class KqlEngineCollectionTest
{
    private List<TestClass> _datas;

    public KqlEngineCollectionTest()
    {
        _datas = new Faker<TestClass>().Generate(20).ToList();
        _datas.Add(new TestClass()
        {
            StringField = "value",
            DateOnlyField = DateOnly.Parse("2022/08/01", CultureInfo.InvariantCulture),
            DateTimeField = DateTime.Parse("2022/08/01", CultureInfo.InvariantCulture),
            NumberField = 10
        });
    }

    public class TestClass
    {
        public string? StringField { get; set; }

        public int NumberField { get; set; }

        public DateTime DateTimeField { get; set; }

        public DateOnly DateOnlyField { get; set; }
    }

    public class KqlStringExpression
    {
        public string? Query { get; set; }

        public Expression<Func<TestClass, bool>>? ExpressionQuery { get; set; }
    }

    internal class QueryDataGenerator : IEnumerable<object[]>
    {
		public static IEnumerable<object[]> GenerateQuery()
        {
			yield return new [] { new KqlStringExpression() { Query = "StringField = 'value'", ExpressionQuery = t => t.StringField == "value" }};
            yield return new [] { new KqlStringExpression() { Query = "StringField = 'value' and NumberField = 10", ExpressionQuery = t => t.StringField == "value" && t.NumberField == 10 }};
			yield return new [] { new KqlStringExpression() { Query = "StringField = 'value' or NumberField != 10", ExpressionQuery = t => t.StringField == "value" || t.NumberField != 10 }};
            yield return new [] { new KqlStringExpression() { Query = "NumberField >= 10", ExpressionQuery = t => t.NumberField >= 10 }};
            yield return new [] { new KqlStringExpression() { Query = "DateTimeField = '2022-08-01' and StringField ='value'", ExpressionQuery = t => t.DateTimeField == DateTime.ParseExact("2022-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture) && t.StringField == "value" }};
            yield return new [] { new KqlStringExpression() { Query = "DateTimeField >= '2022-08-01'", ExpressionQuery = t => t.DateTimeField >= new DateTime(2022,1,1) }};
            yield return new [] { new KqlStringExpression() { Query = "NumberField >= 10 or NumberField = 16", ExpressionQuery = t => t.NumberField >= 10 || t.NumberField == 16 }};
            yield return new [] { new KqlStringExpression() { Query = "NumberField >= 1 and NumberField <= 20", ExpressionQuery = t => t.NumberField >= 1 && t.NumberField <= 20 }};
            yield return new [] { new KqlStringExpression() { Query = "StringField = 'value' and NumberField = 10 and DateTimeField = '2022-08-01'", ExpressionQuery = t => t.StringField == "value" && t.NumberField == 10 && t.DateTimeField == DateTime.ParseExact("2022-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture) }};
        }

		public IEnumerator<object[]> GetEnumerator() => GenerateQuery().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GenerateQuery().GetEnumerator();
    }

    [Trait("Category", "KqlEngine.Collection")]
    [Theory(DisplayName = "Where method should have same result that Linq Where")]
    [MemberData(nameof(QueryDataGenerator.GenerateQuery), MemberType = typeof(QueryDataGenerator))]
    public void KqlQueryWhereMethodShouldHaveSameResult(KqlStringExpression kql)
    {
        // Act
        var kqlResult = _datas.Where(kql.Query!);
        var linqResult = _datas.Where(kql.ExpressionQuery!.Compile());

        // Assert
        Assert.Equal(kqlResult, linqResult);
    }

    [Trait("Category", "KqlEngine.Collection")]
    [Theory(DisplayName = "Any method should have same result that Linq Any()")]
    [MemberData(nameof(QueryDataGenerator.GenerateQuery), MemberType = typeof(QueryDataGenerator))]
    public void KqlQueryAnyMethodShouldHaveSameResult(KqlStringExpression kql)
    {
        // Act
        var kqlResult = _datas.Any(kql.Query!);
        var linqResult = _datas.Any(kql.ExpressionQuery!.Compile());

        // Assert
        Assert.Equal(kqlResult, linqResult);
    }

    [Trait("Category", "KqlEngine.Collection")]
    [Theory(DisplayName = "Count method should have same result that Linq Count()")]
    [MemberData(nameof(QueryDataGenerator.GenerateQuery), MemberType = typeof(QueryDataGenerator))]
    public void KqlQueryCountMethodShouldHaveSameResult(KqlStringExpression kql)
    {
        // Act
        var kqlResult = _datas.Count(kql.Query!);
        var linqResult = _datas.Count(kql.ExpressionQuery!.Compile());

        // Assert
        Assert.Equal(kqlResult, linqResult);
    }

    [Trait("Category", "KqlEngine.Collection")]
    [Fact(DisplayName = "First/FirstOrDefault method should have same result that Linq First/FirstOrDefault()")]
    public void KqlQueryFirstMethodShouldHaveSameResult()
    {
        // Act
        var kqlResult = _datas.First("StringField = 'value' and NumberField = 10");
        var linqResult = _datas.First(t => t.StringField == "value" && t.NumberField == 10);
        var kqlResultOrDefault = _datas.FirstOrDefault("StringField = 'value' and NumberField = 10");
        var linqResultOrDefault = _datas.FirstOrDefault(t => t.StringField == "value" && t.NumberField == 10);
        // Assert
        Assert.Equal(kqlResult, linqResult);
        Assert.Equal(kqlResultOrDefault, linqResultOrDefault);
    }
}