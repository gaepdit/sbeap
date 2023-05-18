using Sbeap.AppServices.Customers.Dto;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkSearchDto
{
    // Sorting
    public CaseworkSortBy Sort { get; init; } = CaseworkSortBy.Customer;

    // Status

    [Display(Name = "Case Status")]
    public CaseStatus? Status { get; init; }

    [Display(Name = "Case Deletion Status")]
    public CaseDeletedStatus? DeletedStatus { get; init; }

    // Case details

    [Display(Name = "Case Description")]
    public string? Description { get; init; }

    // Customer details
    
    [Display(Name = "Customer Name")]
    public string? CustomerName { get; init; }

    [Display(Name = "Customer Deletion Status")]
    public CustomerDeletedStatus? CustomerDeletedStatus { get; init; }

    // Dates

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? OpenedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? OpenedThrough { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedThrough { get; init; }

    // Referral

    [Display(Name = "Referred to")]
    public Guid? ReferralAgency { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReferredFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReferredThrough { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Status), Status?.ToString() },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
        { nameof(CustomerDeletedStatus), CustomerDeletedStatus?.ToString() },
        { nameof(OpenedFrom), OpenedFrom?.ToString("d") },
        { nameof(OpenedThrough), OpenedThrough?.ToString("d") },
        { nameof(ClosedFrom), ClosedFrom?.ToString("d") },
        { nameof(ClosedThrough), ClosedThrough?.ToString("d") },
        { nameof(CustomerName), CustomerName },
        { nameof(Description), Description },
        { nameof(ReferralAgency), ReferralAgency.ToString() },
        { nameof(ReferredFrom), ReferredFrom?.ToString("d") },
        { nameof(ReferredThrough), ReferredThrough?.ToString("d") },
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
    [Description("Customer.Name")] Customer,

    [Description("Customer.Name desc")] CustomerDesc,

    [Description("CaseOpenedDate, Customer.Name")]
    OpenedDate,

    [Description("CaseOpenedDate desc, Customer.Name")]
    OpenedDateDesc,

    [Description("CaseClosedDate, Customer.Name")]
    ClosedDate,

    [Description("CaseClosedDate desc, Customer.Name")]
    ClosedDateDesc,

    [Description("IsDeleted, CaseClosedDate.HasValue, Customer.Name")]
    Status,

    [Description("IsDeleted desc, CaseClosedDate.HasValue desc, Customer.Name")]
    StatusDesc,
}
