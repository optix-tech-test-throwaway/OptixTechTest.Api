using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OptixTechTest.Core.Models;
using OptixTechTest.Core.Services;
using OptixTechTest.Domain.Utilities;

namespace OptixTechTest.Domain.Services;

[SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
public sealed class MovieService : IMovieService
{
    private readonly MoviesDbContext _dbContext;

    public MovieService(MoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PagedListResult<MovieDto>> SearchMoviesAsync(MovieSearchInput searchInput, CancellationToken cancellationToken = default)
    {
        // Apply filters if defined
        var query = _dbContext.Movies
            .WhereIf(!string.IsNullOrEmpty(searchInput.NormalizeQuery()), movie => movie.Title.ToLower().Contains(searchInput.NormalizeQuery()))
            .WhereIf(!string.IsNullOrEmpty(searchInput.NormalizeLanguage()), movie => movie.OriginalLanguage.ToLower() == searchInput.NormalizeLanguage())
            .WhereIf(searchInput.Actors.Length != 0, movie => searchInput.Actors.All(actor => movie.Actors.Contains(actor)))
            .WhereIf(searchInput.Genres.Length != 0, movie => searchInput.Genres.All(genre => movie.Genres.Contains(genre)));

        // Count how many results are available
        var count = await query.CountAsync(cancellationToken);
        
        // Order and paginate results
        var results = await query
            .OrderByPropertyInDirection(searchInput.OrderBy, searchInput.Direction)
            .Skip(searchInput.Cursor)
            .Take(searchInput.Limit)
            .Select(m => m.ToDto())
            .ToListAsync(cancellationToken);
        
        // Calculate the next cursor or set to null if there are no more results
        int? nextCursor = searchInput.Cursor + results.Count;

        if (nextCursor >= count)
        {
            nextCursor = null;
        }
        
        return new PagedListResult<MovieDto>
        {
            Results = results,
            TotalResultsCount = count,
            NextCursor = nextCursor
        };
    }
}
