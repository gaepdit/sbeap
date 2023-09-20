using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.Domain.Entities.Customers;

public class CustomerManager : ICustomerManager
{
    private readonly IContactRepository _contactRepository;
    public CustomerManager(IContactRepository contactRepository) => _contactRepository = contactRepository;

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

    public PhoneNumber CreatePhoneNumber(string number, PhoneType type) =>
        new()
        {
            Id = _contactRepository.GetNextPhoneNumberId(),
            Number = number,
            Type = type,
        };
}
