using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemCreateDto(Guid CaseworkId)
{
    [Required]
    [Display(Name = "Action Type")]
    public Guid? ActionItemTypeId { get; init; }

    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly ActionDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    public string Notes { get; init; } = string.Empty;
}
