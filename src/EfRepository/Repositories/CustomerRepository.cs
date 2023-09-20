using Sbeap.Domain.Entities.Customers;

namespace Sbeap.EfRepository.Repositories;

public sealed class CustomerRepository : BaseRepository<Customer, Guid, AppDbContext>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

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
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
}
