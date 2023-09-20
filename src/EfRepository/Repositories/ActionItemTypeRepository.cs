using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemTypeRepository : NamedEntityRepository<ActionItemType, AppDbContext>, IActionItemTypeRepository
{
    public ActionItemTypeRepository(AppDbContext context) : base(context) { }
}
