namespace OptixTechTest.Core.Models;

public enum MovieOrderBy
{
    ReleaseDate, 
    Title, 
    Popularity, 
    VoteCount, 
    VoteAverage
}

public record MovieSearchInput : SearchInputBase<MovieOrderBy>
{
    public string? Query { get; init; }
    
    public string? Language { get; init; }

    public string[] Genres { get; init; } = [];

    public string[] Actors { get; init; } = [];
    
    public string NormalizeQuery() => Query?.ToLower() ?? string.Empty;
    
    public string NormalizeLanguage() => Language?.ToLower() ?? string.Empty;
}
