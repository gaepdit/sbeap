using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.EfRepository.Repositories;

public sealed class AgencyRepository : BaseRepository<Agency, Guid>, IAgencyRepository
{
    public AgencyRepository(AppDbContext context) : base(context) { }

    public async Task<Agency?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Context.Set<Agency>().AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
