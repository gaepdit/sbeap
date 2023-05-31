using Sbeap.AppServices.DtoBase;

namespace Sbeap.AppServices.ActionItemTypes.Dto;

public record ActionItemTypeCreateDto(string Name) : SimpleNamedEntityCreateDto(Name);

