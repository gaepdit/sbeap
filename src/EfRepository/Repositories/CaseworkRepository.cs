using Sbeap.Domain.Entities.Cases;

namespace Sbeap.EfRepository.Repositories;

public sealed class CaseworkRepository(AppDbContext context)
    : BaseRepository<Casework, Guid, AppDbContext>(context), ICaseworkRepository
{
    public async Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Casework>()
            .Include(e => e.Customer)
            .Include(e => e.ActionItems
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.ActionDate)
                .ThenByDescending(i => i.EnteredOn)
            )
            .Include(e => e.ActionItems)
            .ThenInclude(e => e.EnteredBy)
            .Include(e => e.ReferralAgency)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
}
