using Sbeap.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Cases.Dto;

public record ActionItemViewDto
(
    Guid Id,
    string ActionItemTypeName,
    DateOnly ActionDate,
    string Notes,
    StaffViewDto? EnteredBy,
    [Display(Name = "Entered On")] DateTimeOffset? EnteredOn
);
