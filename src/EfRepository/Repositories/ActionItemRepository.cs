using Sbeap.Domain.Entities.ActionItems;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemRepository : BaseRepository<ActionItem, Guid>, IActionItemRepository
{
    public ActionItemRepository(DbContext context) : base(context) { }
}
