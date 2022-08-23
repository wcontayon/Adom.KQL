using System;
using System.Linq;
using System.Linq.Expressions;
using KqlToLinq;
using KqlToLinq.QueryBuilder;

namespace KqlToLinq.Extensions;

public static class KqlEngineExtensions
{
    /// <summary>
    /// Filters a sequence of values based on a KQL query input.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"><see cref="IEnumerable{T}"/></param>
    /// <param name="kqlQuery">KQL query input</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition</returns>
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, string kqlQuery)
    {
        ArgumentNullException.ThrowIfNull(kqlQuery, nameof(kqlQuery));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var expression = KqlEngine.Parse<T>(kqlQuery);
        return source.Where(expression.Compile());
    }

    /// <summary>
    /// Returns the first element in a sequence that satisfies the KQL query input.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"><see cref="IEnumerable{T}"/></param>
    /// <param name="kqlQuery">KQL query input</param>
    /// <returns>The first element in the sequence that passes the test in the specified KQL query</returns>
    public static T First<T>(this IEnumerable<T> source, string kqlQuery)
    {
        ArgumentNullException.ThrowIfNull(kqlQuery, nameof(kqlQuery));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var expression = KqlEngine.Parse<T>(kqlQuery);
        return source.First(expression.Compile());
    }

    /// <summary>
    /// Returns the first element in a sequence that satisfies the KQL query input.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"><see cref="IEnumerable{T}"/></param>
    /// <param name="kqlQuery">KQL query input</param>
    /// <returns><code>default(T)</code> if source is empty or if no element passes the test specified by the KQL query; 
    /// otherwise, the first element in source that passes the test specified by the KQL query
    /// </returns>
    public static T? FirstOrDefault<T>(this IEnumerable<T> source, string kqlQuery)
    {
        ArgumentNullException.ThrowIfNull(kqlQuery, nameof(kqlQuery));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var expression = KqlEngine.Parse<T>(kqlQuery);
        return source.FirstOrDefault(expression.Compile());
    }
}