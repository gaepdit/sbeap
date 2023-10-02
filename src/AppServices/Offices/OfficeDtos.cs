using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.Offices;

public record OfficeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record OfficeCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record OfficeUpdateDto(string Name, bool Active) : StandardNamedEntityUpdateDto(Name, Active);
