using System.ComponentModel;
using System.Linq.Expressions;
using OptixTechTest.Core.Models;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain.Utilities;

public static class QueryableExtensions
{    
    private static readonly Dictionary<MovieOrderBy, Expression<Func<Movie, object>>> PropertyMap = new()
    {
        [MovieOrderBy.ReleaseDate] = m => m.ReleaseDate,
        [MovieOrderBy.Title] = m => m.Title,
        [MovieOrderBy.Popularity] = m => m.Popularity,
        [MovieOrderBy.VoteCount] = m => m.VoteCount,
        [MovieOrderBy.VoteAverage] = m => m.VoteAverage
    };

    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

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