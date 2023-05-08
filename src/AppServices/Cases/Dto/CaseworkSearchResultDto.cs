using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public class CaseworkSearchResultDto
{
    public Guid Id { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public DateOnly CaseOpenedDate { get; init; }
    public DateOnly? CaseClosedDate { get; init; }
    public string Description { get; init; } = string.Empty;

    [UIHint("BoolClosed")]
    public bool IsClosed { get; init; }

    public bool IsDeleted { get; init; }
}
