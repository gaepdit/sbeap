namespace Sbeap.Domain.Entities.Agencies;

public class AgencyManager(IAgencyRepository repository)
    : NamedEntityManager<Agency, IAgencyRepository>(repository), IAgencyManager;
