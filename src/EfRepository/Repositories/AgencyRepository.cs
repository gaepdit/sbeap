using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.EfRepository.Repositories;

public sealed class AgencyRepository : NamedEntityRepository<Agency>, IAgencyRepository
{
    public AgencyRepository(DbContext context) : base(context) { }
}
