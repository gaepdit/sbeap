using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalActionItemTypeRepository : NamedEntityRepository<ActionItemType>, IActionItemTypeRepository
{
    public LocalActionItemTypeRepository() : base(ActionItemTypeData.GetActionItemTypes) { }
}
