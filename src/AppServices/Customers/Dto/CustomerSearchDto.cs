using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerSearchDto
{
    public SortBy Sort { get; init; } = SortBy.NameAsc;

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? County { get; set; }

    [Display(Name = "Deletion Status")]
    public CaseDeletedStatus? DeletedStatus { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Name), Name },
        { nameof(Description), Description },
        { nameof(County), County },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
    };

    public void TrimAll()
    {
        Name = Name?.Trim();
        Description = Description?.Trim();
        County = County?.Trim();
    }
}

// Search enums

// "Not Deleted" is included as an additional Delete Status option in the UI representing the default state.
// "Deleted" = only deleted customers
// "All" = all customers
// "Not Deleted" (null) = only non-deleted customers
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseDeletedStatus
{
    Deleted = 0,
    All = 1,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("Name")] NameAsc,
    [Description("Name desc")] NameDesc,
    [Description("Description")] DescriptionAsc,
    [Description("Description desc")] DescriptionDesc,
}
