using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.Cases;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCaseworkRepository : BaseRepository<Casework, Guid>, ICaseworkRepository
{
    private readonly IActionItemRepository _actionItemRepository;

    public LocalCaseworkRepository(IActionItemRepository actionItemRepository)
        : base(CaseworkData.GetCases)
    {
        _actionItemRepository = actionItemRepository;
    }

    public async Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default)
    {
        var result = await FindAsync(id, token);
        if (result is null) return result;

        result.ActionItems = (await _actionItemRepository
                .GetListAsync(e => e.Casework.Id == id && !e.IsDeleted, token))
            .OrderByDescending(i => i.EnteredOn)
            .ToList();

        return result;
    }
}
