using System.ComponentModel;
using System.Linq.Expressions;
using OptixTechTest.Core.Models;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain.Utilities;

/// <summary>
/// Provides extension methods for IQueryable to enhance querying capabilities.
/// </summary>
public static class QueryableExtensions
{    
    /// <summary>
    /// Maps MovieOrderBy enum values to corresponding property expressions for sorting.
    /// </summary>
    private static readonly Dictionary<MovieOrderBy, Expression<Func<Movie, object>>> PropertyMap = new()
    {
        [MovieOrderBy.ReleaseDate] = m => m.ReleaseDate,
        [MovieOrderBy.Title] = m => m.Title,
        [MovieOrderBy.Popularity] = m => m.Popularity,
        [MovieOrderBy.VoteCount] = m => m.VoteCount,
        [MovieOrderBy.VoteAverage] = m => m.VoteAverage
    };

    /// <summary>
    /// Conditionally applies a Where clause to the query based on a specified condition.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in source.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}"/> to apply the where clause to.</param>
    /// <param name="condition">A boolean condition that determines whether to apply the filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// Orders the query by a specified property and sort direction.
    /// </summary>
    /// <param name="source">An <see cref="IQueryable{Movie}"/> to apply the ordering to.</param>
    /// <param name="property">The property to order by, as defined in <see cref="MovieOrderBy"/>.</param>
    /// <param name="direction">The sort direction (ascending or descending).</param>
    /// <returns>An <see cref="IQueryable{Movie}"/> with the ordering applied.</returns>
    public static IQueryable<Movie> OrderByPropertyInDirection(
        this IQueryable<Movie> source,
        MovieOrderBy property,
        ListSortDirection direction)
    {
        if (!PropertyMap.TryGetValue(property, out var selector))
        {
            return source;
        }

        return direction == ListSortDirection.Descending
            ? source.OrderByDescending(selector)
            : source.OrderBy(selector);
    }
}