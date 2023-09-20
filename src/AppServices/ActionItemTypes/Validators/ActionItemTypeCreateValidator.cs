using FluentValidation;
using Sbeap.Domain;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes.Validators;

public class ActionItemTypeCreateValidator : AbstractValidator<ActionItemTypeCreateDto>
{
    private readonly IActionItemTypeRepository _repository;

    public ActionItemTypeCreateValidator(IActionItemTypeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
