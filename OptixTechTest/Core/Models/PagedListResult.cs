namespace OptixTechTest.Core.Models;

/// <summary>
/// Represents a paginated result set containing a collection of items and metadata about pagination.
/// </summary>
/// <typeparam name="T">The type of items in the result collection.</typeparam>
public record PagedListResult<T>
{
    /// <summary>
    /// Gets the collection of items for the current page.
    /// </summary>
    /// <value>A list of items of type <typeparamref name="T"/></value>
    public List<T> Results { get; init; } = [];

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    /// <value>An integer representing the total count of available items.</value>
    public int TotalResultsCount { get; init; }

    /// <summary>
    /// Gets the cursor for the next page of results, if available.
    /// </summary>
    /// <value>An integer cursor value for retrieving the next page, or null if there are no more pages.</value>
    public int? NextCursor { get; init; }
}