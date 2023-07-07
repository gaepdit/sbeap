using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.Agencies;

public record AgencyCreateDto(string Name) : SimpleNamedEntityCreateDto(Name);

public record AgencyUpdateDto(Guid Id, string Name, bool Active) : SimpleNamedEntityUpdateDto(Id, Name, Active);

public record AgencyViewDto(Guid Id, string Name, bool Active) : SimpleNamedEntityViewDto(Id, Name, Active);
