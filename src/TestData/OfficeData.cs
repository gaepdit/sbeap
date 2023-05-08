using Sbeap.Domain.Entities.Offices;
using Sbeap.TestData.Identity;

namespace Sbeap.TestData;

internal static class OfficeData
{
    private static IEnumerable<Office> OfficeSeedItems => new List<Office>
    {
        new(new Guid("10000000-0000-0000-0000-000000000001"), "SBEAP") { Active = true },
    };

    private static IEnumerable<Office>? _offices;

    public static IEnumerable<Office> GetOffices
    {
        get
        {
            if (_offices is not null) return _offices;
            _offices = OfficeSeedItems.ToList();

            foreach (var office in _offices)
            {
                office.StaffMembers = UserData.GetUsers
                    .Where(e => e.Office?.Id == office.Id).ToList();
            }

            return _offices;
        }
    }

    public static void ClearData() => _offices = null;
}
