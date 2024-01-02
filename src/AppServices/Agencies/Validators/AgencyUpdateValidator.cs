using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies.Validators;

public class AgencyUpdateValidator(IAgencyRepository repository) :
    StandardNamedEntityUpdateValidator<Agency, AgencyUpdateDto, IAgencyRepository>(repository);
