﻿using GaEpd.AppLibrary.ListItems;

namespace Sbeap.AppServices.ActionItemTypes;

public interface IActionItemTypeService : IDisposable, IAsyncDisposable
{
    Task<IReadOnlyList<ActionItemTypeViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(ActionItemTypeCreateDto resource, CancellationToken token = default);
    Task<ActionItemTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(Guid id, ActionItemTypeUpdateDto resource, CancellationToken token = default);
}
