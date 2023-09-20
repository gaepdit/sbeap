using Sbeap.Domain.Entities.ActionItems;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemRepository : BaseRepository<ActionItem, Guid, AppDbContext>, IActionItemRepository
{
    public ActionItemRepository(AppDbContext context) : base(context) { }
}
