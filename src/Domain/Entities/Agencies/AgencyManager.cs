namespace Sbeap.Domain.Entities.Agencies;

public class AgencyManager : NamedEntityManager<Agency, IAgencyRepository>, IAgencyManager
{
    public AgencyManager(IAgencyRepository repository) : base(repository) { }
}
