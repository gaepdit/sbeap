using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.Offices;

public record OfficeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record OfficeCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record OfficeUpdateDto(Guid Id, string Name, bool Active) : StandardNamedEntityUpdateDto(Id, Name, Active);
