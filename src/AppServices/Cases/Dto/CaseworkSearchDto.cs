using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkSearchDto
{
    // Sorting
    public SortBy Sort { get; init; } = SortBy.CustomerAsc;

    // Status

    [Display(Name = "Case Status")]
    public CaseStatus? Status { get; init; }

    [Display(Name = "Deletion Status")]
    public SearchDeleteStatus? DeletedStatus { get; init; }

    // Dates

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? OpenedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? OpenedTo { get; init; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ClosedTo { get; init; }

    // Fields

    public string? CustomerName { get; init; } 

    public string? Description { get; set; }

    [Display(Name = "Referred to")]
    public Guid? Agency { get; init; }

    // Referral

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReferredFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReferredTo { get; init; }

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

    public void TrimAll()
    {
        Description = Description?.Trim();
    }
}

// Search enums

// "All" is included as an additional Case Status option in the UI representing the default state.
public enum CaseStatus
{
    Open,
    Closed,
}

// "Not Deleted" is included as an additional Delete Status option in the UI representing the default state.
// "Deleted" = only deleted cases
// "All" = all cases
// "Not Deleted" (null) = only non-deleted cases
public enum SearchDeleteStatus
{
    Deleted = 0,
    All = 1,
}

public enum SortBy
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
