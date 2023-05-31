using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.ActionItemTypes.Dto;

public record ActionItemTypeUpdateDto(Guid Id, string Name, bool Active) : SimpleNamedEntityUpdateDto(Id, Name, Active);
