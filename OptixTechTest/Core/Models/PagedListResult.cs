namespace OptixTechTest.Core.Models;

public record PagedListResult<T>
{
    public List<T>? Results { get; init; }

    public int TotalResultsCount { get; init; }

    public int? NextCursor { get; init; }
}