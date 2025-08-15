using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCustomerRepository(
    IContactRepository contactRepository,
    ICaseworkRepository caseworkRepository)
    : BaseRepository<Customer, Guid>(CustomerData.GetCustomers), ICustomerRepository
{
    public async Task<Customer?> FindIncludeAllAsync(
        Guid id, bool includeDeletedCases, CancellationToken token = default)
    {
        var result = await FindAsync(id, token).ConfigureAwait(false);
        if (result is null) return result;

        result.Contacts = (await contactRepository
                .GetListAsync(e => e.Customer.Id == id && !e.IsDeleted, token).ConfigureAwait(false))
            .OrderByDescending(i => i.EnteredOn)
            .ToList();

        result.Cases = (await caseworkRepository
                .GetListAsync(e => e.Customer.Id == id && (includeDeletedCases || !e.IsDeleted), token)
                .ConfigureAwait(false))
            .OrderByDescending(i => i.CaseOpenedDate)
            .ToList();

        return result;
    }
}
