using OptixTechTest.Core.Models;

namespace OptixTechTest.Core.Services;

public interface IMovieService
{
    Task<PagedListResult<MovieDto>> SearchMoviesAsync(MovieSearchInput searchInput, CancellationToken cancellationToken = default);
}
