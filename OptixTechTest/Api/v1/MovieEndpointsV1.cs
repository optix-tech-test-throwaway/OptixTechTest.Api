using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OptixTechTest.Api.v1.Filters;
using OptixTechTest.Core.Models;
using OptixTechTest.Core.Services;

namespace OptixTechTest.Api.v1;

/// <summary>
/// Contains API endpoint definitions for movie-related operations in API v1.
/// </summary>
public static class MovieEndpointsV1
{
    /// <summary>
    /// Maps all movie-related API endpoints for the v1 version of the API.
    /// </summary>
    /// <param name="group">The route group builder to add the endpoints to.</param>
    public static RouteHandlerBuilder MapMoviesApiV1(this RouteGroupBuilder group)
    {
        return group
            .MapPost("search", SearchMovies)
            .AddEndpointFilter<ValidateMovieSearchInputFilter>()
            .ProducesValidationProblem()
            .WithName("SearchMovies");
    }

    /// <summary>
    /// Handles the movie search operation.
    /// </summary>
    /// <param name="movieService">The movie service used to perform the search operation.</param>
    /// <param name="searchInput">The search criteria provided by the client.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// An HTTP 200 OK response containing a paged list of movies that match the search criteria.
    /// </returns>
    public static async Task<Ok<PagedListResult<MovieDto>>> SearchMovies(
        [FromServices]IMovieService movieService, 
        [FromBody]MovieSearchInput searchInput, 
        CancellationToken cancellationToken)
    {
        var movies = await movieService.SearchMoviesAsync(searchInput, cancellationToken);
        return TypedResults.Ok(movies);
    }
}
