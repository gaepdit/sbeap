using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.Cases.Dto;

public class ActionItemCreateDto
{
    public Guid Casework { get; init; }
    public ActionItemType ActionItemType { get; private init; } = default!;
    public DateOnly ActionDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}
