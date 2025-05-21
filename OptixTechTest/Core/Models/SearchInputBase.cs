using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OptixTechTest.Core.Models;

public abstract record SearchInputBase<TOrderBy>
    where TOrderBy : Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ListSortDirection Direction { get; init; } = ListSortDirection.Ascending;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TOrderBy OrderBy { get; init; } = default!;

    [DefaultValue(0)]
    public int Cursor { get; init; }

    [DefaultValue(20)]
    public int Limit { get; init; } = 20;
}
