using Sbeap.Domain.Entities.Contacts;

namespace Sbeap.Domain.Entities.Customers;

/// <inheritdoc />
public class CustomerManager : ICustomerManager
{
    public Customer Create(string name, string? createdById)
    {
        var item = new Customer(Guid.NewGuid()) { Name = name };
        item.SetCreator(createdById);
        return item;
    }

    public Contact CreateContact(Customer customer, string? createdById)
    {
        var item = new Contact(Guid.NewGuid(), customer) { EnteredOn = DateTimeOffset.Now };
        item.SetCreator(createdById);
        return item;
    }
}
