using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.ActionItemTypes;

public record ActionItemTypeCreateDto(string Name) : SimpleNamedEntityCreateDto(Name);

public record ActionItemTypeUpdateDto(Guid Id, string Name, bool Active) : SimpleNamedEntityUpdateDto(Id, Name, Active);

public record ActionItemTypeViewDto(Guid Id, string Name, bool Active) : SimpleNamedEntityViewDto(Id, Name, Active);
