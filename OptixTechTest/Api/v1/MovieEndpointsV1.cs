using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OptixTechTest.Api.v1.Filters;
using OptixTechTest.Core.Models;
using OptixTechTest.Core.Services;

namespace OptixTechTest.Api.v1;

public static class MovieEndpointsV1
{
    public static void MapMoviesApiV1(this RouteGroupBuilder group)
    {
        group.MapPost("search", SearchMovies)
            .AddEndpointFilter<ValidateMovieSearchInputFilter>()
            .WithName("SearchMovies");
    }

    public static async Task<Ok<PagedListResult<MovieDto>>> SearchMovies(
        [FromServices]IMovieService movieService, 
        [FromBody]MovieSearchInput searchInput, 
        CancellationToken cancellationToken)
    {
        var movies = await movieService.SearchMoviesAsync(searchInput, cancellationToken);
        return TypedResults.Ok(movies);
    }
}
