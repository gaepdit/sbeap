using Sbeap.Domain.Entities.ActionItems;
using Sbeap.EfRepository.Contexts;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemRepository : BaseRepository<ActionItem, Guid>, IActionItemRepository
{
    public ActionItemRepository(AppDbContext context) : base(context) { }
}
