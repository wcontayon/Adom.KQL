
using System.Linq.Expressions;

namespace KqlToLinq;

/// <summary>
/// Engine to process a kql query.
/// </summary>
public class KqlEngine 
{
    [ThreadStatic]
    private static Grammar? _grammar;

    /// <summary>
    /// Parse a KQL query input, and build the <see cref="Expression{Fun{T, bool}}" /> tree
    /// to be executed using LINQ.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input">KQL query</param>
    /// <returns><see cref="Expression{Fun{T, bool}}" /> corresponding to the KQL query</returns>
    public static Expression<Func<T, bool>> Parse<T>(string input) => ParseQuery<T>(input);

    /// <summary>
    /// Parse a KQL query input, and build the <see cref="Expression{Fun{T, bool}}" /> tree
    /// to be executed using LINQ.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input">KQL query</param>
    /// <param name="paramName">Expression parameter</param>
    /// <returns><see cref="Expression{Fun{T, bool}}" /> corresponding to the KQL query</returns>
    public static Expression<Func<T, bool>> Parse<T>(string input, string paramName) => ParseQuery<T>(input, paramName);

    /// <summary>
    /// Process a KQL query, build the predicate to filter the source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"><see cref="IEnumerable{T}">.</param>
    /// <param name="kqlQuery">The KQL query to process</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition</returns>
    public static IEnumerable<T> ProcessQuery<T>(IEnumerable<T> source, string kqlQuery)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(kqlQuery, nameof(kqlQuery));

        var expression = ParseQuery<T>(kqlQuery);
        return source.Where(expression.Compile());
    }

    private static Expression<Func<T, bool>> ParseQuery<T>(string input, string paramName = "p")
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }
        // Init the <see cref="Grammar" />
        EnsureGrammarInstance();

        var queryBuilder = new QueryBuilder.QueryBuilder(typeof(T), paramName);

        // Tokenize the query input
        var tokens = _grammar!._lexer.Tokenize(input);

        // Parse and build expression tree
        var expression = _grammar!._parser.Parse(tokens, queryBuilder);

        return Expression.Lambda<Func<T, bool>>(expression, queryBuilder.Parameter);
    }

    private static void EnsureGrammarInstance()
    {
        if (_grammar == null)
        {
            _grammar = Grammar.Instance();
        }
    }
}