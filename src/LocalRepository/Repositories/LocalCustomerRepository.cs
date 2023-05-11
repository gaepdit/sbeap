using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public LocalCustomerRepository() : base(CustomerData.GetCustomers) { }

    public async Task<Customer?> FindIncludeAllAsync(Guid id, CancellationToken token = default)
    {
        var results = await FindAsync(id, token);
        if (results is null) return results;
        results.Contacts.RemoveAll(e => e.IsDeleted);
        return results;
    }
}
