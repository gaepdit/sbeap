using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.Cases;
using Sbeap.EfRepository.Contexts;

namespace Sbeap.EfRepository.Repositories;

public sealed class CaseworkRepository : BaseRepository<Casework, Guid>, ICaseworkRepository
{
    public CaseworkRepository(AppDbContext context) : base(context) { }

    public async Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Casework>()
            .Include(e => e.Customer)
            .Include(e => e.ActionItems.Where(i => !i.IsDeleted))
            .Include(e => e.ActionItems).ThenInclude(e => e.ActionItemType)
            .Include(e => e.ActionItems).ThenInclude(e => e.EnteredBy)
            .Include(e => e.ReferralAgency)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
}
