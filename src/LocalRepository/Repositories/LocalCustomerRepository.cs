using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public LocalCustomerRepository() : base(CustomerData.GetCustomers) { }

    public async Task<Customer?> FindIncludeAllAsync(
        Guid id, bool includeDeletedCases, CancellationToken token = default)
    {
        var results = await FindAsync(id, token);
        if (results is null) return results;

        results.Contacts.RemoveAll(contact => contact.IsDeleted);
        results.Contacts = results.Contacts.OrderByDescending(i => i.EnteredOn).ToList();

        if (!includeDeletedCases) results.Cases.RemoveAll(casework => casework.IsDeleted);
        results.Cases = results.Cases.OrderByDescending(i => i.CaseOpenedDate).ToList();

        return results;
    }
}
