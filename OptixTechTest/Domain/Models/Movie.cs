using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OptixTechTest.Domain.Models;

/// <summary>
/// Represents a movie entity with various metadata.
/// </summary>
[Index(nameof(Title))]
[Index(nameof(ReleaseDate))]
[Index(nameof(Popularity))]
[Index(nameof(VoteCount))]
[Index(nameof(VoteAverage))]
public class Movie
{
    /// <summary>
    /// Gets the unique identifier for the movie.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the release date of the movie.
    /// </summary>
    public DateOnly ReleaseDate { get; init; }

    /// <summary>
    /// Gets the title of the movie.
    /// Maximum length is 256 characters.
    /// </summary>
    [MaxLength(256)]
    public string Title { get; init; } = null!;

    /// <summary>
    /// Gets the plot summary or description of the movie.
    /// Maximum length is 2048 characters.
    /// </summary>
    [MaxLength(2048)]
    public string Overview { get; init; } = null!;

    /// <summary>
    /// Gets the popularity rating of the movie.
    /// Higher values indicate more popular movies.
    /// </summary>
    public decimal Popularity { get; init; }

    /// <summary>
    /// Gets the total number of votes received for the movie.
    /// </summary>
    public uint VoteCount { get; init; }

    /// <summary>
    /// Gets the average vote score for the movie.
    /// </summary>
    public decimal VoteAverage { get; init; }

    /// <summary>
    /// Gets the original language code of the movie.
    /// Uses ISO 639-1 two-letter language codes.
    /// Maximum length is 2 characters.
    /// </summary>
    [MaxLength(2)]
    public string OriginalLanguage { get; init; } = null!;

    /// <summary>
    /// Gets the list of genres associated with the movie.
    /// </summary>
    public List<string> Genres { get; init; } = [];

    /// <summary>
    /// Gets the list of actors who performed in the movie.
    /// </summary>
    public List<string> Actors { get; init; } = [];

    /// <summary>
    /// Gets the URL for the movie's poster image.
    /// Maximum length is 128 characters.
    /// </summary>
    [MaxLength(128)]
    public string PosterUrl { get; init; } = null!;
}
