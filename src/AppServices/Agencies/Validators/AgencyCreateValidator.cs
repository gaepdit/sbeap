using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies.Validators;

public class AgencyCreateValidator(IAgencyRepository repository) :
    StandardNamedEntityCreateValidator<Agency, AgencyCreateDto, IAgencyRepository>(repository);
