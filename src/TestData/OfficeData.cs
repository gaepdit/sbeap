using Sbeap.Domain.Entities.Offices;

namespace Sbeap.TestData;

internal static class OfficeData
{
    private static IEnumerable<Office> OfficeSeedItems => new List<Office>
    {
        new(new Guid("10000000-0000-0000-0000-000000000001"), "Branch"),
        new(new Guid("10000000-0000-0000-0000-000000000002"), "District"),
        new(new Guid("10000000-0000-0000-0000-000000000003"), "Region"),
        new(new Guid("10000000-0000-0000-0000-000000000004"), "Closed Office") { Active = false },
    };

    private static IEnumerable<Office>? _offices;

    public static IEnumerable<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;
            _offices = OfficeSeedItems;
            return _offices;
        }
    }

    public static void ClearData() => _offices = null;
}
