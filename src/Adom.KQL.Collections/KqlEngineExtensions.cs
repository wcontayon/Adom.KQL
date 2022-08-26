// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

namespace Adom.KQL.Collections;

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

    /// <summary>
    /// Determines whether any element of a sequence satisfies the KQL query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"><see cref="IEnumerable{T}"/></param>
    /// <param name="kqlQuery">KQL query input</param>
    /// <returns><code>true</code if the source sequence is not empty and at least one of its elements passes the test in the specified KQL query; otherwise, <code>false</code>.
    /// </returns>
    public static bool Any<T>(this IEnumerable<T> source, string kqlQuery)
    {
        ArgumentNullException.ThrowIfNull(kqlQuery, nameof(kqlQuery));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var expression = KqlEngine.Parse<T>(kqlQuery);
        return source.Any(expression.Compile());
    }

    /// <summary>
    /// Returns a number that represents how many elements in the specified sequence satisfy the KQL query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"><see cref="IEnumerable{T}"/></param>
    /// <param name="kqlQuery">KQL query input</param>
    /// <returns>A number that represents how many elements in the sequence satisfy the condition in the KQL query</returns>
    public static int Count<T>(this IEnumerable<T> source, string kqlQuery)
    {
        ArgumentNullException.ThrowIfNull(kqlQuery, nameof(kqlQuery));
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        var expression = KqlEngine.Parse<T>(kqlQuery);
        return source.Count(expression.Compile());
    }
}