using Sbeap.Domain.Exceptions;

namespace Sbeap.Domain.Entities.ActionItemTypes;

/// <inheritdoc />
public class ActionItemTypeManager : IActionItemTypeManager
{
    private readonly IActionItemTypeRepository _repository;
    public ActionItemTypeManager(IActionItemTypeRepository repository) => _repository = repository;

    public async Task<ActionItemType> CreateAsync(string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, ignoreId: null, token);
        return new ActionItemType(Guid.NewGuid(), name);
    }

    public async Task ChangeNameAsync(ActionItemType actionItemType, string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, actionItemType.Id, token);
        actionItemType.ChangeName(name);
    }

    private async Task ThrowIfDuplicateName(string name, Guid? ignoreId, CancellationToken token)
    {
        // Validate the name is not a duplicate
        var existing = await _repository.FindByNameAsync(name.Trim(), token);
        if (existing is not null && (ignoreId is null || existing.Id != ignoreId))
            throw new NameAlreadyExistsException(name);
    }
}
