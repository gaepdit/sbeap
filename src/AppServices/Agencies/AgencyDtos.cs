using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.Agencies;

public record AgencyCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record AgencyUpdateDto(Guid Id, string Name, bool Active) : StandardNamedEntityUpdateDto(Id, Name, Active);

public record AgencyViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);
