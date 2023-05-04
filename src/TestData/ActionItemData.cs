using Sbeap.Domain.Entities.ActionItems;
using Sbeap.TestData.Constants;
using Sbeap.TestData.Identity;

namespace Sbeap.TestData;

internal static class ActionItemData
{
    private static IEnumerable<ActionItem> ActionItemSeedItems => new List<ActionItem>
    {
        new(new Guid("60000000-0000-0000-0000-000000000001"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(6))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            Notes = TextData.MultipleParagraphs,
        },
        new(new Guid("60000000-0000-0000-0000-000000000002"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            Notes = string.Empty,
        },
    };

    private static IEnumerable<ActionItem>? _actionItems;

    public static IEnumerable<ActionItem> GetActionItems
    {
        get
        {
            if (_actionItems is not null) return _actionItems;
            _actionItems = ActionItemSeedItems;
            return _actionItems;
        }
    }

    public static void ClearData() => _actionItems = null;
}
