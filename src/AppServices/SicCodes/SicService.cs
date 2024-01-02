using GaEpd.AppLibrary.ListItems;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.AppServices.SicCodes;

public sealed class SicService(ISicRepository repository) : ISicService
{
    public async Task<IReadOnlyList<ListItem<string>>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await repository.GetListAsync(token)).Select(sic => new ListItem<string>(sic.Id, Name: sic.Display)).ToList();

    void IDisposable.Dispose() => repository.Dispose();
}
