using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemUpdateDto
{
    // Authorization handler assist properties

    public bool IsDeleted { get; [UsedImplicitly] init; }
    public bool CaseworkIsDeleted { get; [UsedImplicitly] init; }
    public Guid CaseWorkId { get; [UsedImplicitly] init; }

    // Entity update properties

    [Required]
    [Display(Name = "Action Type")]
    public Guid? ActionItemTypeId { get; [UsedImplicitly] init; }

    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly ActionDate { get; [UsedImplicitly] init; }

    [Required]
    public string Notes { get; [UsedImplicitly] init; } = string.Empty;
}
