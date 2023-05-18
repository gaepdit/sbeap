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
            ReferralAgency = AgencyData.GetAgencies.ElementAt(0),
            ReferralNotes = TextData.ShortPhrase,
            ReferralDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
        },
        new(new Guid("50000000-0000-0000-0000-000000000002"),
            CustomerData.GetCustomers.ElementAt(0),
            DateOnly.FromDateTime(DateTime.Today))
        {
            CaseClosedDate = null,
            Description = string.Empty,
            ReferralAgency = null,
            ReferralNotes = string.Empty,
            ReferralDate = null,
        },
        new(new Guid("50000000-0000-0000-0000-000000000003"),
            CustomerData.GetCustomers.ElementAt(2),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-10)))
        {
            CaseClosedDate = null,
            Description = "An open case for a deleted customer.",
            ReferralAgency = null,
            ReferralNotes = string.Empty,
            ReferralDate = null,
        },
        new(new Guid("50000000-0000-0000-0000-000000000004"),
            CustomerData.GetCustomers.ElementAt(1),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-10)))
        {
            Description = "A Deleted Case",
        },
    };

    private static IEnumerable<Casework>? _cases;

    public static IEnumerable<Casework> GetCases
    {
        get
        {
            if (_cases is not null) return _cases;
            _cases = CaseworkSeedItems.ToList();
            _cases.ElementAt(3).SetDeleted("00000000-0000-0000-0000-000000000001");

            foreach (var casework in _cases)
            {
                casework.ActionItems = ActionItemData.GetActionItems
                    .Where(e => e.Casework.Id == casework.Id).ToList();
            }

            return _cases;
        }
    }

    public static void ClearData() => _cases = null;
}
