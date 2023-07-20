using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.EfRepository.Contexts;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemTypeRepository : BaseRepository<ActionItemType, Guid>, IActionItemTypeRepository
{
    public ActionItemTypeRepository(AppDbContext context) : base(context) { }

    public async Task<ActionItemType?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Context.ActionItemTypes.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
