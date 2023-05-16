using Sbeap.AppServices.Staff.Dto;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemViewDto
(
    Guid Id,
    string ActionItemTypeName,
    DateOnly ActionDate,
    string Notes,
    StaffViewDto EnteredBy,
    bool IsDeleted
);
