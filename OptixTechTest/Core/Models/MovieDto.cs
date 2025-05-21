namespace OptixTechTest.Core.Models;

/// <summary>
/// Represents a data transfer object for a movie.
/// </summary>
public record MovieDto
{
    /// <summary>
    /// Gets the unique identifier for the movie.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Gets the release date of the movie.
    /// </summary>
    public DateOnly ReleaseDate { get; init; }

    /// <summary>
    /// Gets the title of the movie.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// Gets a brief overview or description of the movie.
    /// </summary>
    public string Overview { get; init; } = null!;

    /// <summary>
    /// Gets the popularity score of the movie.
    /// </summary>
    public decimal Popularity { get; init; }

    /// <summary>
    /// Gets the total number of votes received for the movie.
    /// </summary>
    public uint VoteCount { get; init; }

    /// <summary>
    /// Gets the average vote rating for the movie.
    /// </summary>
    public decimal VoteAverage { get; init; }

    /// <summary>
    /// Gets the original language in which the movie was produced.
    /// </summary>
    public string OriginalLanguage { get; init; } = null!;

    /// <summary>
    /// Gets an array of genres associated with the movie.
    /// </summary>
    public string[] Genres { get; init; } = [];
    
    /// <summary>
    /// Gets an array of actors who starred in the movie.
    /// </summary>
    public string[] Actors { get; init; } = [];

    /// <summary>
    /// Gets the URL to the movie's poster image.
    /// </summary>
    public string PosterUrl { get; init; } = null!;
}
