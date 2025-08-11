using FluentValidation;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using Sbeap.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Sbeap.AppServices.DtoBase;

[SuppressMessage("", "S2436")]
public abstract class StandardNamedEntityUpdateValidator<TEntity, TDto, TRepository> : AbstractValidator<TDto>
    where TEntity : IEntity, INamedEntity
    where TDto : INamedEntity
    where TRepository : INamedEntityRepository<TEntity>
{
    private readonly TRepository _repository;

    protected StandardNamedEntityUpdateValidator(TRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, context, token) => await NotDuplicateName(name, context, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, ValidationContext<TDto> context,
        CancellationToken token = default)
    {
        var existing = await _repository.FindByNameAsync(name, token);
        return existing is null || existing.Id == (Guid)context.RootContextData[nameof(existing.Id)];
    }
}
