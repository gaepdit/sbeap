using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public LocalCustomerRepository() : base(CustomerData.GetCustomers) { }
}
