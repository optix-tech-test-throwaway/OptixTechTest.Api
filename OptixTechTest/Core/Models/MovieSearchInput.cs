namespace OptixTechTest.Core.Models;

/// <summary>
/// Specifies the ordering options for movie search results.
/// </summary>
public enum MovieOrderBy
{
    /// <summary>
    /// Order by movie release date.
    /// </summary>
    ReleaseDate, 
    
    /// <summary>
    /// Order by movie title alphabetically.
    /// </summary>
    Title, 
    
    /// <summary>
    /// Order by movie popularity rating.
    /// </summary>
    Popularity, 
    
    /// <summary>
    /// Order by number of votes received.
    /// </summary>
    VoteCount, 
    
    /// <summary>
    /// Order by average vote score.
    /// </summary>
    VoteAverage
}

/// <summary>
/// Represents input parameters for searching movies.
/// Inherits from SearchInputBase with MovieOrderBy as the ordering type.
/// </summary>
public record MovieSearchInput : SearchInputBase<MovieOrderBy>
{
    /// <summary>
    /// Gets the search query text.
    /// </summary>
    public string? Query { get; init; }
    
    /// <summary>
    /// Gets the preferred language for search results.
    /// </summary>
    public string? Language { get; init; }

    /// <summary>
    /// Gets a collection of genre filters.
    /// </summary>
    public string[] Genres { get; init; } = [];

    /// <summary>
    /// Gets a collection of actor filters.
    /// </summary>
    public string[] Actors { get; init; } = [];
    
    /// <summary>
    /// Normalizes the query string by converting it to lowercase.
    /// </summary>
    /// <returns>The lowercase query string or an empty string if Query is null.</returns>
    public string NormalizeQuery() => Query?.ToLower() ?? string.Empty;
    
    /// <summary>
    /// Normalizes the language string by converting it to lowercase.
    /// </summary>
    /// <returns>The lowercase language string or an empty string if Language is null.</returns>
    public string NormalizeLanguage() => Language?.ToLower() ?? string.Empty;
}
