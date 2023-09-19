using FluentValidation;
using Sbeap.Domain.Entities;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies.Validators;

public class AgencyCreateValidator : AbstractValidator<AgencyCreateDto>
{
    private readonly IAgencyRepository _repository;

    public AgencyCreateValidator(IAgencyRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(SbeapStandardNamedEntity.MinimumNameLength, SbeapStandardNamedEntity.MaximumNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
