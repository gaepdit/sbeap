namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkSearchResultDto
(
    Guid Id,
    string CustomerName,
    DateOnly CaseOpenedDate,
    DateOnly? CaseClosedDate,
    string Description,
    bool IsClosed,
    bool IsDeleted
);
