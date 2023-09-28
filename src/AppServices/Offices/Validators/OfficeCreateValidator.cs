using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.Offices.Validators;

public class OfficeCreateValidator : StandardNamedEntityCreateValidator<Office, OfficeCreateDto, IOfficeRepository>
{
    public OfficeCreateValidator(IOfficeRepository repository) : base(repository) { }
}
