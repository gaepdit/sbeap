namespace Sbeap.AppServices.Cases.Dto;

public class CaseworkCreateDto
{
    public Guid CustomerId { get; init; }

    public DateOnly CaseOpenedDate { get; init; }
    public string Description { get; init; } = string.Empty;
}
