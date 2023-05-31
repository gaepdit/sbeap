using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    private readonly IContactRepository _contactRepository;
    private readonly ICaseworkRepository _caseworkRepository;

    public LocalCustomerRepository(
        IContactRepository contactRepository,
        ICaseworkRepository caseworkRepository)
        : base(CustomerData.GetCustomers)
    {
        _contactRepository = contactRepository;
        _caseworkRepository = caseworkRepository;
    }

    public async Task<Customer?> FindIncludeAllAsync(
        Guid id, bool includeDeletedCases, CancellationToken token = default)
    {
        var result = await FindAsync(id, token);
        if (result is null) return result;

        result.Contacts = (await _contactRepository
                .GetListAsync(e => e.Customer.Id == id && !e.IsDeleted, token))
            .OrderByDescending(i => i.EnteredOn)
            .ToList();

        result.Cases = (await _caseworkRepository
                .GetListAsync(e => e.Customer.Id == id && (includeDeletedCases || !e.IsDeleted), token))
            .OrderByDescending(i => i.CaseOpenedDate)
            .ToList();

        return result;
    }
}
