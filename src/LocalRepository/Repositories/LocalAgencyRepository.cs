using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalAgencyRepository : NamedEntityRepository<Agency>, IAgencyRepository
{
    public LocalAgencyRepository() : base(AgencyData.GetAgencies) { }
}
