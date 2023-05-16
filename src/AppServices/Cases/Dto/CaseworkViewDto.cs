using Sbeap.AppServices.Customers.Dto;

namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkViewDto
(
    Guid Id,
    CustomerSearchResultDto Customer,
    string Description,
    DateOnly CaseOpenedDate,
    DateOnly? CaseClosedDate,
    bool IsClosed,
    string? CaseClosureNotes,
    string? ReferralAgencyName,
    DateOnly? ReferralDate,
    string? ReferralNotes,
    ICollection<ActionItemViewDto> ActionItems,
    bool IsDeleted
);
