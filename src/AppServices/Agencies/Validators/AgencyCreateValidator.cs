using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies.Validators;

public class AgencyCreateValidator : StandardNamedEntityCreateValidator<Agency, AgencyCreateDto, IAgencyRepository>
{
    public AgencyCreateValidator(IAgencyRepository repository) : base(repository) { }
}
