namespace Sbeap.AppServices.Customers.Dto;

public record CustomerSearchResultDto
(
    Guid Id,
    string Name,
    string Description,
    string? County,
    bool IsDeleted
);
