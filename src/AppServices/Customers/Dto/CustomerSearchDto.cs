using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerSearchDto
{
    public CustomerSortBy Sort { get; init; } = CustomerSortBy.Name;
    public string? Name { get; init; }
    public string? Description { get; init; }

    [Display(Name = "SIC Code")]
    public string? SicCode { get; init; }

    public string? City { get; init; }
    public string? County { get; init; }

    [Display(Name = "Deletion Status")]
    public CustomerDeletedStatus? DeletedStatus { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Name), Name },
        { nameof(Description), Description },
        { nameof(SicCode), SicCode },
        { nameof(City), City },
        { nameof(County), County },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
    };

    public CustomerSearchDto TrimAll() => this with
    {
        Name = Name?.Trim(),
        Description = Description?.Trim(),
        SicCode = SicCode?.Trim(),
        City = City?.Trim(),
        County = County?.Trim(),
    };
}

// Search enums

// "Not Deleted" is included as an additional Delete Status option in the UI representing the default state.
// "Deleted" = only deleted customers
// "All" = all customers
// "Not Deleted" (null) = only non-deleted customers
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerDeletedStatus
{
    Deleted = 0,
    All = 1,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerSortBy
{
    [Description("Name")] Name,
    [Description("Name desc")] NameDesc,
    [Description("Description")] Description,
    [Description("Description desc")] DescriptionDesc,
}
