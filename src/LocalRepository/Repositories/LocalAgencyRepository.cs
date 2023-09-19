using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalAgencyRepository : BaseRepository<Agency, Guid>, IAgencyRepository
{
    public LocalAgencyRepository() : base(AgencyData.GetAgencies) { }

    public async Task<Agency?> FindByNameAsync(string name, CancellationToken token = default) =>
        await FindAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
