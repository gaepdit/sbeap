using GaEpd.AppLibrary.ListItems;

namespace Sbeap.AppServices.SicCodes;

public interface ISicService : IDisposable
{
    Task<IReadOnlyList<ListItem<string>>> GetActiveListItemsAsync(CancellationToken token = default);
}
