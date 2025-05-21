using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OptixTechTest.Core.Models;

/// <summary>
/// Base abstract record for search input parameters that supports pagination and sorting.
/// </summary>
/// <typeparam name="TOrderBy">An enum type that defines the available sorting fields.</typeparam>
public abstract record SearchInputBase<TOrderBy>
    where TOrderBy : Enum
{
    /// <summary>
    /// Gets the sorting direction.
    /// </summary>
    /// <value>
    /// The direction to sort results. Defaults to <see cref="ListSortDirection.Ascending"/>.
    /// </value>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ListSortDirection Direction { get; init; } = ListSortDirection.Ascending;

    /// <summary>
    /// Gets the field to order results by.
    /// </summary>
    /// <value>
    /// The enum value representing the field to sort by.
    /// </value>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TOrderBy OrderBy { get; init; } = default!;

    /// <summary>
    /// Gets the starting position for pagination.
    /// </summary>
    /// <value>
    /// The zero-based index from which to start returning results. Defaults to 0.
    /// </value>
    [DefaultValue(0)]
    public int Cursor { get;init; }

    /// <summary>
    /// Gets the maximum number of results to return.
    /// </summary>
    /// <value>
    /// The maximum number of items to return in a single request. Defaults to 20.
    /// </value>
    [DefaultValue(20)]
    public int Limit { get; init; } = 20;
}
