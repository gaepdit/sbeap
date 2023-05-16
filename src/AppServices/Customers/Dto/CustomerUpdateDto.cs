using Sbeap.Domain.ValueObjects;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerUpdateDto
(
    Guid Id,
    string Name,
    string Description,
    string? County,
    IncompleteAddress Location,
    IncompleteAddress MailingAddress
);
