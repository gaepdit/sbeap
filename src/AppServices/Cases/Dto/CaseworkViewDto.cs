using JetBrains.Annotations;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkViewDto
{
    public Guid Id { get; init; }
    public CustomerSearchResultDto Customer { get; init; } = default!;
    public string Description { get; init; } = string.Empty;

    [Display(Name = "Opened")]
    public DateOnly CaseOpenedDate { get; init; }

    [Display(Name = "Closed")]
    public DateOnly? CaseClosedDate { get; init; }

    [Display(Name = "Status")]
    public bool IsClosed { get; init; }

    [Display(Name = "Closure Notes")]
    public string? CaseClosureNotes { get; init; }

    [Display(Name = "Referred To")]
    public string? ReferralAgencyName { get; init; }

    [Display(Name = "Date")]
    public DateOnly? ReferralDate { get; init; }

    [Display(Name = "Notes")]
    public string? ReferralNotes { get; init; }

    [UsedImplicitly]
    public List<ActionItemViewDto> ActionItems { get; init; } = new();

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }
}
