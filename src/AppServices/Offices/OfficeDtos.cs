using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.Offices;

public record OfficeViewDto(Guid Id, string Name, bool Active) : SimpleNamedEntityViewDto(Id, Name, Active);

public record OfficeCreateDto(string Name) : SimpleNamedEntityCreateDto(Name);

public record OfficeUpdateDto(Guid Id, string Name, bool Active) : SimpleNamedEntityUpdateDto(Id, Name, Active);
