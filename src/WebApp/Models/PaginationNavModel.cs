using GaEpd.AppLibrary.Pagination;

namespace Sbeap.WebApp.Models;

public class PaginationNavModel(IPaginatedResult paging, IDictionary<string, string?> routeValues)
{
    public IPaginatedResult Paging => paging;
    public IDictionary<string, string?> RouteValues => routeValues;
}
