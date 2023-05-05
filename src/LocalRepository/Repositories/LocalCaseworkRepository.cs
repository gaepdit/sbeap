using Sbeap.Domain.Entities.Cases;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalCaseworkRepository : BaseRepository<Casework, Guid>, ICaseworkRepository
{
    public LocalCaseworkRepository() : base(CaseworkData.GetCases) { }
}
