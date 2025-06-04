using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.Cases;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCaseworkRepository(IActionItemRepository actionItemRepository)
    : BaseRepository<Casework, Guid>(CaseworkData.GetCases), ICaseworkRepository
{
    public async Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default)
    {
        var result = await FindAsync(id, token);
        if (result is null) return result;

        result.ActionItems = (await actionItemRepository
                .GetListAsync(e => e.Casework.Id == result.Id && !e.IsDeleted, token))
            .OrderByDescending(i => i.ActionDate)
            .ThenByDescending(i => i.EnteredOn)
            .ToList();

        return result;
    }
}
