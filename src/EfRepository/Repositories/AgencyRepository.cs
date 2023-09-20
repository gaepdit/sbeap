using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.EfRepository.Repositories;

public sealed class AgencyRepository : NamedEntityRepository<Agency, AppDbContext>, IAgencyRepository
{
    public AgencyRepository(AppDbContext context) : base(context) { }
}
