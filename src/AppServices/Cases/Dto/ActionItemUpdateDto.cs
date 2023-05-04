using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.Cases.Dto;

public class ActionItemUpdateDto
{
    public Guid Id { get; init; }
    public ActionItemType ActionItemType { get; private init; } = default!;
    public DateOnly ActionDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}
