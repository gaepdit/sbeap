using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.EfRepository.Repositories;

public sealed class AgencyRepository(AppDbContext context)
    : NamedEntityRepository<Agency, AppDbContext>(context), IAgencyRepository;
