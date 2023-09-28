using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.Offices.Validators;

public class OfficeUpdateValidator : StandardNamedEntityUpdateValidator<Office, OfficeUpdateDto, IOfficeRepository>
{
    public OfficeUpdateValidator(IOfficeRepository repository) : base(repository) { }
}
