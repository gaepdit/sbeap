using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.Customers;
using Sbeap.EfRepository.Contexts;

namespace Sbeap.EfRepository.Repositories;

public sealed class CustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> FindIncludeAllAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Customer>()
            .Include(e => e.Contacts.Where(i => !i.IsDeleted))
            .Include(e => e.Cases.Where(i => !i.IsDeleted))
            .Include(e => e.Cases).ThenInclude(e => e.ReferralAgency)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
}
