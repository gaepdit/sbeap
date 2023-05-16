using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkSearchDto
(
    // Sorting
    CaseworkSortBy Sort,

    // Status
    [Display(Name = "Case Status")] CaseStatus? Status,
    [Display(Name = "Deletion Status")] CaseDeletedStatus? DeletedStatus,

    // Dates
    [Display(Name = "From")]
    [DataType(DataType.Date)]
    // [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    DateOnly? OpenedFrom,
    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    // [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    DateOnly? OpenedTo,
    [Display(Name = "From")]
    [DataType(DataType.Date)]
    // [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    DateOnly? ClosedFrom,
    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    // [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    DateOnly? ClosedTo,

    // Fields
    string? CustomerName,
    string? Description,
    [Display(Name = "Referred to")] Guid? Agency,

    // Referral
    [Display(Name = "From")]
    [DataType(DataType.Date)]
    // [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    DateOnly? ReferredFrom,
    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    // [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    DateOnly? ReferredTo
)
{
    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Status), Status?.ToString() },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
        { nameof(OpenedFrom), OpenedFrom?.ToString("d") },
        { nameof(OpenedTo), OpenedTo?.ToString("d") },
        { nameof(ClosedFrom), ClosedFrom?.ToString("d") },
        { nameof(ClosedTo), ClosedTo?.ToString("d") },
        { nameof(CustomerName), CustomerName },
        { nameof(Description), Description },
        { nameof(Agency), Agency.ToString() },
        { nameof(ReferredFrom), ReferredFrom?.ToString("d") },
        { nameof(ReferredTo), ReferredTo?.ToString("d") },
    };

    public CaseworkSearchDto TrimAll() => this with
    {
        Description = Description?.Trim(),
    };
}

// Search enums

// "All" is included as an additional Case Status option in the UI representing the default state.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseStatus
{
    Open,
    Closed,
}

// "Not Deleted" is included as an additional Delete Status option in the UI representing the default state.
// "Deleted" = only deleted cases
// "All" = all cases
// "Not Deleted" (null) = only non-deleted cases
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseDeletedStatus
{
    Deleted = 0,
    All = 1,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseworkSortBy
{
    [Description("Customer.Name")] CustomerAsc,

    [Description("Customer.Name desc")] CustomerDesc,

    [Description("CaseOpenedDate, Customer.Name")]
    OpenedDateAsc,

    [Description("CaseOpenedDate desc, Customer.Name")]
    OpenedDateDesc,

    [Description("CaseClosedDate, Customer.Name")]
    ClosedDateAsc,

    [Description("CaseClosedDate desc, Customer.Name")]
    ClosedDateDesc,
}
