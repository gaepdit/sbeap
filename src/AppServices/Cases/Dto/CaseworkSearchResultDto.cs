using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkSearchResultDto
{
    public Guid Id { get; [UsedImplicitly] init; }

    [Display(Name = "Customer")]
    public string CustomerName { get; init; } = string.Empty;

    [Display(Name = "Date Opened")]
    public DateOnly CaseOpenedDate { get; init; }

    [Display(Name = "Date Closed")]
    public DateOnly? CaseClosedDate { get; init; }

    public string Description { get; init; } = string.Empty;

    public bool IsClosed { get; [UsedImplicitly] init; }

    public bool IsDeleted { get; [UsedImplicitly] init; }
}
