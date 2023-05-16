namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkUpdateDto
(
    Guid Id,
    DateOnly CaseOpenedDate,
    string Description,
    DateOnly? CaseClosedDate,
    string? CaseClosureNotes,
    Guid? ReferralAgencyId,
    DateOnly? ReferralDate,
    string? ReferralNotes
);
