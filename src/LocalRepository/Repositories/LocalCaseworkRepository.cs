using Sbeap.Domain.Entities.Cases;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCaseworkRepository : BaseRepository<Casework, Guid>, ICaseworkRepository
{
    public LocalCaseworkRepository() : base(CaseworkData.GetCases) { }

    public async Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default)
    {
        var results = await FindAsync(id, token);
        if (results is null) return results;
        results.ActionItems.RemoveAll(e => e.IsDeleted);
        return results;
    }
}
