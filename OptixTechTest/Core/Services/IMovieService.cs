using OptixTechTest.Core.Models;

namespace OptixTechTest.Core.Services;

/// <summary>
/// Service interface for retrieving movie information from the data source.
/// </summary>
public interface IMovieService
{
    /// <summary>
    /// Searches for movies based on the provided search criteria.
    /// </summary>
    /// <param name="searchInput">The search parameters to filter the results by.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a paged list of movies matching the search criteria.
    /// </returns>
    Task<PagedListResult<MovieDto>> SearchMoviesAsync(MovieSearchInput searchInput, CancellationToken cancellationToken = default);
}
