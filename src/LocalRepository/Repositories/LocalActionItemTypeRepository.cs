using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalActionItemTypeRepository : BaseRepository<ActionItemType, Guid>, IActionItemTypeRepository
{
    public LocalActionItemTypeRepository() : base(ActionItemTypeData.GetActionItemTypes) { }

    public async Task<ActionItemType?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));
}
