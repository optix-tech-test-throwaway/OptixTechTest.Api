using OptixTechTest.Core.Models;
using OptixTechTest.Domain.Models;

namespace OptixTechTest.Domain.Utilities;

/// <summary>
/// Provides extension methods for mapping between <see cref="Movie"/> entities and <see cref="MovieDto"/> data transfer objects.
/// </summary>
public static class MovieMapper
{
    /// <summary>
    /// Converts a <see cref="Movie"/> entity to a <see cref="MovieDto"/> data transfer object.
    /// </summary>
    /// <param name="movie">The movie entity to convert.</param>
    /// <returns>A new instance of <see cref="MovieDto"/> containing the movie data.</returns>
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

    /// <summary>
    /// Converts a <see cref="MovieDto"/> data transfer object to a <see cref="Movie"/> entity.
    /// </summary>
    /// <param name="movie">The movie DTO to convert.</param>
    /// <returns>A new instance of <see cref="Movie"/> entity representing the movie data.</returns>
    /// <remarks>
    /// If the Id in the DTO is null, it will be set to <see cref="Guid.Empty"/> in the entity, and later automatically assigned by the database.
    /// </remarks>
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