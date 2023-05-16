using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemCreateDto
(
    Guid CaseworkId,
    ActionItemType ActionItemType,
    DateOnly ActionDate,
    string Notes
);
