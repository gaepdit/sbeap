using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkUpdateDto
{
    // Authorization handler assist properties
    public bool IsDeleted { get; [UsedImplicitly] init; }
    public bool CustomerIsDeleted { get; [UsedImplicitly] init; }

    public Guid Id { get; [UsedImplicitly] init; }

    [Display(Name = "Date Opened")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly CaseOpenedDate { get; [UsedImplicitly] init; }

    public string Description { get; [UsedImplicitly] init; } = string.Empty;

    [Display(Name = "Date Closed")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? CaseClosedDate { get; [UsedImplicitly] init; }

    [Display(Name = "Closure Notes")]
    public string? CaseClosureNotes { get; [UsedImplicitly] init; }

    [Display(Name = "Referred To")]
    public Guid? ReferralAgencyId { get; [UsedImplicitly] init; }

    [Display(Name = "Date Referred")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReferralDate { get; [UsedImplicitly] init; }

    [Display(Name = "Referral Notes")]
    public string? ReferralNotes { get; [UsedImplicitly] init; }
}
