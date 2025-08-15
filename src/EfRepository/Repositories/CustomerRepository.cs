using Sbeap.Domain.Entities.Customers;

namespace Sbeap.EfRepository.Repositories;

public sealed class CustomerRepository(AppDbContext context)
    : BaseRepository<Customer, Guid, AppDbContext>(context), ICustomerRepository
{
    public async Task<Customer?> FindIncludeAllAsync(
        Guid id, bool includeDeletedCases, CancellationToken token = default) =>
        await Context.Set<Customer>()
            .Include(e => e.Contacts
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.EnteredOn))
            .ThenInclude(e => e.EnteredBy)
            .Include(e => e.Cases
                .Where(i => includeDeletedCases || !i.IsDeleted)
                .OrderByDescending(item => item.CaseOpenedDate))
            .ThenInclude(e => e.ReferralAgency)
            .AsSplitQuery()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token).ConfigureAwait(false);
}
