namespace Sbeap.AppServices.Customers.Dto;

public class CustomerSearchResultDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int NumberOfCases { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? County { get; init; }
    public bool IsDeleted { get; init; }
}
