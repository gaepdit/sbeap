using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public LocalCustomerRepository() : base(CustomerData.GetCustomers) { }

    public Task<Customer?> FindIncludeAllAsync(Guid id, CancellationToken token = default) => FindAsync(id, token);
}
