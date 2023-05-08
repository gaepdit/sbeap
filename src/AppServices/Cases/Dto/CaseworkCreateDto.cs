using Sbeap.Domain.Entities.Customers;

namespace Sbeap.AppServices.Cases.Dto;

public class CaseworkCreateDto
{
    public Customer Customer { get; init; } = default!;

    public DateOnly CaseOpenedDate { get; init; }
    public string Description { get; init; } = string.Empty;
}
