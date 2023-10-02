using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.ActionItemTypes;

public record ActionItemTypeCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record ActionItemTypeUpdateDto(string Name, bool Active) : StandardNamedEntityUpdateDto(Name, Active);

public record ActionItemTypeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);
