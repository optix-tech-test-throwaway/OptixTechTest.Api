using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OptixTechTest.Domain.Models;

[Index(nameof(Title))]
[Index(nameof(ReleaseDate))]
[Index(nameof(Popularity))]
[Index(nameof(VoteCount))]
[Index(nameof(VoteAverage))]
public class Movie
{
    public Guid Id { get; init; }

    public DateOnly ReleaseDate { get; init; }

    [MaxLength(256)]
    public string Title { get; init; } = null!;

    [MaxLength(2048)]
    public string Overview { get; init; } = null!;

    public decimal Popularity { get; init; }

    public uint VoteCount { get; init; }

    public decimal VoteAverage { get; init; }

    [MaxLength(2)]
    public string OriginalLanguage { get; init; } = null!;

    public List<string> Genres { get; init; } = [];

    public List<string> Actors { get; init; } = [];

    [MaxLength(128)]
    public string PosterUrl { get; init; } = null!;
}
