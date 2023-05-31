using Sbeap.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemViewDto
{
    public Guid Id { get; [UsedImplicitly] init; }

    [Display(Name = "Action Type")]
    public string ActionItemTypeName { get; init; } = string.Empty;

    [Display(Name = "Action Date")]
    public DateOnly ActionDate { get; init; }
    public string Notes { get; init; } = string.Empty;
    public StaffViewDto? EnteredBy { get; [UsedImplicitly] init; }

    [Display(Name = "Entered On")]
    public DateTimeOffset? EnteredOn { get; init; }
}
