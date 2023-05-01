namespace Sbeap.Domain.Entities.Customers;

/// <inheritdoc />
public class CustomerManager : ICustomerManager
{
    public Customer Create() => new(Guid.NewGuid());
}
