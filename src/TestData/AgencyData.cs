using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.TestData;

internal static class AgencyData
{
    private static IEnumerable<Agency> AgencySeedItems => new List<Agency>
    {
        new(new Guid("20000000-0000-0000-0000-000000000001"), "Albany") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000002"), "APB"),
        new(new Guid("20000000-0000-0000-0000-000000000003"), "Athens") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000004"), "Atlanta District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000005"), "Atlanta") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000006"), "Augusta") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000007"), "Brunswick District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000008"), "Brunswick") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000009"), "Cartersville District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000010"), "Cartersville") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000011"), "Coastal District (Brunswick)"),
        new(new Guid("20000000-0000-0000-0000-000000000012"), "Columbus") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000013"), "Directors Agency") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000014"), "East Central Dist (Augusta)"),
        new(new Guid("20000000-0000-0000-0000-000000000015"), "East Central District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000016"), "EPA") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000017"), "EPD Air Nox") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000018"), "EPD Hazardous Waste") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000019"), "EPD NE District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000020"), "GDED"),
        new(new Guid("20000000-0000-0000-0000-000000000021"), "GEFA"),
        new(new Guid("20000000-0000-0000-0000-000000000022"), "HWB") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000023"), "LBP"),
        new(new Guid("20000000-0000-0000-0000-000000000024"), "Macon") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000025"), "Main Agency") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000026"), "Mountain Dist (Cartersville)"),
        new(new Guid("20000000-0000-0000-0000-000000000027"), "Mountain District (Atlanta)"),
        new(new Guid("20000000-0000-0000-0000-000000000028"), "Northeast District (Athens)"),
        new(new Guid("20000000-0000-0000-0000-000000000029"), "Northeast District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000030"), "NSBEAP"),
        new(new Guid("20000000-0000-0000-0000-000000000031"), "P2AD"),
        new(new Guid("20000000-0000-0000-0000-000000000032"), "Savannah") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000033"), "SBEAP") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000034"), "Southwest District (Albany)"),
        new(new Guid("20000000-0000-0000-0000-000000000035"), "Southwest District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000036"), "Statewide"),
        new(new Guid("20000000-0000-0000-0000-000000000037"), "West Central District (Macon)"),
        new(new Guid("20000000-0000-0000-0000-000000000038"), "West Central District") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000039"), "WPB"),
    };

    private static IEnumerable<Agency>? _agencies;

    public static IEnumerable<Agency> GetAgencies
    {
        get
        {
            if (_agencies is not null) return _agencies;
            _agencies = AgencySeedItems;
            return _agencies;
        }
    }

    public static void ClearData() => _agencies = null;
}
