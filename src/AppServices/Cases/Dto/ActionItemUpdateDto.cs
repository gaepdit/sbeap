using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemUpdateDto(
    Guid Id,
    ActionItemType ActionItemType,
    DateOnly ActionDate,
    string Notes
);
