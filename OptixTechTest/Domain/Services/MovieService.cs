using Microsoft.EntityFrameworkCore;
using OptixTechTest.Core.Models;
using OptixTechTest.Core.Services;
using OptixTechTest.Domain.Utilities;

namespace OptixTechTest.Domain.Services;

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
            .WhereIf(!string.IsNullOrEmpty(searchInput.NormalizeQuery()), 
                movie => EF.Functions.ILike(movie.Title, $"%{EscapeLikePattern(searchInput.NormalizeQuery())}%"))
            .WhereIf(!string.IsNullOrEmpty(searchInput.NormalizeLanguage()), 
                movie => movie.OriginalLanguage == searchInput.NormalizeLanguage())
            .WhereIf(searchInput.Actors.Length != 0, 
                movie => searchInput.Actors.All(actor => movie.Actors.Contains(actor)))
            .WhereIf(searchInput.Genres.Length != 0, 
                movie => searchInput.Genres.All(genre => movie.Genres.Contains(genre)))
            .WhereIf(searchInput.MinPopularity.HasValue,
                movie => movie.Popularity >= searchInput.MinPopularity)
            .WhereIf(searchInput.MaxPopularity.HasValue, 
                movie => movie.Popularity <= searchInput.MaxPopularity)
            .WhereIf(searchInput.MinVoteAverage.HasValue, 
                movie => movie.VoteAverage >= searchInput.MinVoteAverage)
            .WhereIf(searchInput.MaxVoteAverage.HasValue, 
                movie => movie.VoteAverage <= searchInput.MaxVoteAverage)
            .WhereIf(searchInput.MinVoteCount.HasValue,
                movie => movie.VoteCount >= searchInput.MinVoteCount)
            .WhereIf(searchInput.MaxVoteCount.HasValue, 
                movie => movie.VoteCount <= searchInput.MaxVoteCount);

        // Count how many results are available
        var count = await query.CountAsync(cancellationToken);
        
        // Order and paginate results
        var results = await query
            .OrderByPropertyInDirection(searchInput.OrderBy, searchInput.Direction)
            .Skip(searchInput.Cursor)
            .Take(searchInput.Limit)
            .Select(m => m.ToDto())
            .AsNoTracking()
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

    private static string EscapeLikePattern(string input)
    {
        // Escape special characters: \ % _
        return input
            .Replace("\\", @"\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");
    }
}
