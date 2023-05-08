using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Contacts;

/// <inheritdoc />
public class ContactManager : IContactManager
{
    public Contact Create(Customer customer) => new(Guid.NewGuid(), customer);
}
