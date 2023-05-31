using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemUpdateDto
{
    // Authorization handler assist properties

    public bool IsDeleted { get; [UsedImplicitly] init; }
    public bool CaseIsDeleted { get; [UsedImplicitly] init; }

    // Entity update properties

    public Guid Id { get; [UsedImplicitly] init; }

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
