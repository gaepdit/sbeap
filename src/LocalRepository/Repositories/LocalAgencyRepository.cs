using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalAgencyRepository : BaseRepository<Agency, Guid>, IAgencyRepository
{
    public LocalAgencyRepository() : base(AgencyData.GetAgencies) { }

    public Task<Agency?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));
}
