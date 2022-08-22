using KqlToLinq.Syntax;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("KqlToLinq.Tests")]
namespace KqlToLinq.QueryBuilder;

internal class QueryBuilder : IQueryExpressionBuilder
{
    private readonly ParameterExpression _paramExpression;
    private readonly Expression? _expression;
    private readonly PropertyInfo[] _properties;

    public QueryBuilder(Type type)
    {
        _paramExpression = Expression.Parameter(type, "p");
        _properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    }

    public QueryBuilder(Type type, string parameterName)
    {
        _paramExpression = Expression.Parameter(type, parameterName);
        _properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    }

    /// <inheritdoc />
    public Expression EvaluateBinaryOperand(QueryOperandKind operandKind, Expression left, Expression right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return operandKind switch
        {
            QueryOperandKind.And => Expression.AndAlso(left, right),
            _ => Expression.OrElse(left, right),
        };
    }

    /// <inheritdoc />
    public Expression EvaluateQuerySyntax(QuerySyntax syntax)
    {
        PropertyInfo? property = _properties.FirstOrDefault(p => p.Name == syntax.LeftToken.Text! && p.MemberType == MemberTypes.Property);
        
        if (property == null)
        {
            ThrowHelpers.UnknownFieldInQuery(syntax.LeftToken.Text!, _paramExpression.Type.Name);
        }

        MemberExpression memberExpression = Expression.Property(_paramExpression, syntax.LeftToken.Text!);
        ConstantExpression constantExpression = syntax.RightToken.Kind switch
        {
            TokenKind.DateTimeValue => Expression.Constant(DateTime.ParseExact(syntax.RightToken.Text!, "yyyy-MM-dd", CultureInfo.InvariantCulture), typeof(DateTime)),
            TokenKind.BoolValue => Expression.Constant(bool.Parse(syntax.RightToken.Text!), typeof(bool)),
            TokenKind.NumberValue => Expression.Constant(Convert.ChangeType(syntax.RightToken.Text!, property.PropertyType)),
            TokenKind.StringValue => Expression.Constant(syntax.RightToken?.Text!, typeof(string)),
            _ => Expression.Constant(syntax.RightToken?.Text!, typeof(string))
        };

        if (syntax.Operator != OperatorKind.StartWith &&
            syntax.Operator != OperatorKind.EndWith &&
            syntax.Operator != OperatorKind.Contains &&
            syntax.Operator != OperatorKind.NotEquals &&
            syntax.Operator != OperatorKind.LessThan &&
            syntax.Operator != OperatorKind.LessThanOrEqual &&
            syntax.Operator != OperatorKind.GreaterThan &&
            syntax.Operator != OperatorKind.GreaterThanOrEqual &&
            syntax.Operator != OperatorKind.Equals)
        {
            ThrowHelpers.UnknownOperator(syntax.Text.Text!);
        }

        return syntax.Operator switch
        {
            OperatorKind.LessThan => Expression.LessThan(memberExpression, constantExpression),
            OperatorKind.LessThanOrEqual => Expression.LessThanOrEqual(memberExpression, constantExpression),
            OperatorKind.GreaterThan => Expression.GreaterThan(memberExpression, constantExpression),
            OperatorKind.GreaterThanOrEqual => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
            OperatorKind.Equals => Expression.Equal(memberExpression, constantExpression),
            OperatorKind.NotEquals => Expression.NotEqual(memberExpression, constantExpression),
            OperatorKind.Contains => throw new NotImplementedException(),
            OperatorKind.StartWith => throw new NotImplementedException(),
            OperatorKind.EndWith => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }
}
