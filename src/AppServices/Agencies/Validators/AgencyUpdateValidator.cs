using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies.Validators;

public class AgencyUpdateValidator : StandardNamedEntityUpdateValidator<Agency, AgencyUpdateDto, IAgencyRepository>
{
    public AgencyUpdateValidator(IAgencyRepository repository) : base(repository) { }
}
