namespace Sbeap.AppServices.Cases.Dto;

public class CaseworkUpdateDto
{
    public Guid Id { get; init; }

    public DateOnly CaseOpenedDate { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateOnly? CaseClosedDate { get; init; }
    public string? CaseClosureNotes { get; init; }
    public Guid? ReferralAgencyId { get; init; }
    public DateOnly? ReferralDate { get; init; }
    public string? ReferralNotes { get; init; }
}
