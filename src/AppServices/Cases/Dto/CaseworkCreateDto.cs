using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkCreateDto(Guid CustomerId)
{
    [Display(Name = "Date Opened")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly CaseOpenedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    public string? Description { get; init; } = string.Empty;
}
