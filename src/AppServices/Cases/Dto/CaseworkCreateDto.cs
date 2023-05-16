namespace Sbeap.AppServices.Cases.Dto;

public record CaseworkCreateDto
(
    Guid CustomerId,
    DateOnly CaseOpenedDate,
    string Description
);
