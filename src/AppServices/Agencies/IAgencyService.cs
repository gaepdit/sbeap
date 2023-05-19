using GaEpd.AppLibrary.ListItems;

namespace Sbeap.AppServices.Agencies;

public interface IAgencyService : IDisposable
{
    Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool activeOnly = true, CancellationToken token = default);
}
