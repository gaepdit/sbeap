using Sbeap.Domain.Entities.Contacts;

namespace Sbeap.Domain.Entities.Customers;

/// <inheritdoc />
public class CustomerManager : ICustomerManager
{
    public Customer Create(string name) => new(Guid.NewGuid()) { Name = name };
    public Contact CreateContact(Customer customer) => new(Guid.NewGuid(), customer);
}
