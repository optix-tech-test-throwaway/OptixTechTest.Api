namespace OptixTechTest.Core.Models;

public record MovieDto
{
    public Guid? Id { get; init; }

    public DateOnly ReleaseDate { get; init; }

    public string Title { get; init; } = null!;

    public string Overview { get; init; } = null!;

    public decimal Popularity { get; init; }

    public uint VoteCount { get; init; }

    public decimal VoteAverage { get; init; }

    public string OriginalLanguage { get; init; } = null!;

    public string[] Genres { get; init; } = [];
    
    public string[] Actors { get; init; } = [];

    public string PosterUrl { get; init; } = null!;
}
