using OptixTechTest.Core.Models;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain.Utilities;

public static class MovieMapper
{
    public static MovieDto ToDto(this Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        Overview = movie.Overview,
        ReleaseDate = movie.ReleaseDate,
        PosterUrl = movie.PosterUrl,
        Genres = movie.Genres.ToArray(),
        Actors = movie.Actors.ToArray(),
        VoteAverage = movie.VoteAverage,
        VoteCount = movie.VoteCount,
        Popularity = movie.Popularity,
        OriginalLanguage = movie.OriginalLanguage
    };

    public static Movie ToEntity(this MovieDto movie) => new()
    {
        Id = movie.Id ?? Guid.Empty,
        Title = movie.Title,
        Overview = movie.Overview,
        ReleaseDate = movie.ReleaseDate,
        PosterUrl = movie.PosterUrl,
        Genres = movie.Genres.ToList(),
        Actors = movie.Actors.ToList(),
        VoteAverage = movie.VoteAverage,
        VoteCount = movie.VoteCount,
        Popularity = movie.Popularity,
        OriginalLanguage = movie.OriginalLanguage
    };
}
