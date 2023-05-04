using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.AppServices.Customers.Dto;

public class CustomerViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? County { get; init; }
    public IncompleteAddress Location { get; init; } = default!;
    public IncompleteAddress MailingAddress { get; init; } = default!;
    public ICollection<ContactViewDto> Contacts { get; } = new List<ContactViewDto>();
    public bool IsDeleted { get; init; }
}
