using Sbeap.AppServices.Staff.Dto;

namespace Sbeap.AppServices.Cases.Dto;

public class ActionItemViewDto
{
    public Guid Id { get; init; }
    public string ActionItemTypeName { get; private init; } = default!;
    public DateOnly ActionDate { get; init; }
    public string Notes { get; init; } = string.Empty;
    public StaffViewDto EnteredBy { get; init; } = default!;
    public bool IsDeleted { get; init; }
}
