using Sbeap.Domain.Entities.Cases;
using Sbeap.TestData.Constants;

namespace Sbeap.TestData;

internal static class CaseworkData
{
    private static IEnumerable<Casework> CaseworkSeedItems => new List<Casework>
    {
        new(new Guid("50000000-0000-0000-0000-000000000001"),
            CustomerData.GetCustomers.ElementAt(0),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-4)))
        {
            CaseClosedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            Description = TextData.MultipleParagraphs,
            InteragencyReferral = AgencyData.GetAgencies.ElementAt(0),
            ReferralInformation = TextData.ShortPhrase,
            ReferralDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
        },
        new(new Guid("50000000-0000-0000-0000-000000000002"),
            CustomerData.GetCustomers.ElementAt(0),
            DateOnly.FromDateTime(DateTime.Today))
        {
            CaseClosedDate = null,
            Description = string.Empty,
            InteragencyReferral = null,
            ReferralInformation = string.Empty,
            ReferralDate = null,
        },
        new(new Guid("50000000-0000-0000-0000-000000000003"),
            CustomerData.GetCustomers.ElementAt(1),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-10)))
        {
            CaseClosedDate = null,
            Description = TextData.EmojiWord,
            InteragencyReferral = null,
            ReferralInformation = string.Empty,
            ReferralDate = null,
        },
    };

    private static IEnumerable<Casework>? _cases;

    public static IEnumerable<Casework> GetCases
    {
        get
        {
            if (_cases is not null) return _cases;
            _cases = CaseworkSeedItems;
            return _cases;
        }
    }

    public static void ClearData() => _cases = null;
}
