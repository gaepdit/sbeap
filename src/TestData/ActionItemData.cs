using Sbeap.Domain.Entities.ActionItems;
using Sbeap.TestData.Constants;
using Sbeap.TestData.Identity;

namespace Sbeap.TestData;

internal static class ActionItemData
{
    private static IEnumerable<ActionItem> ActionItemSeedItems => new List<ActionItem>
    {
        new(new Guid("51000000-0000-0000-0000-000000000001"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(6))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
            EnteredBy = UserData.GetUsers.ElementAt(0),
            EnteredOn = DateTimeOffset.Now.AddDays(-3),
            Notes = TextData.MultipleParagraphs,
        },
        new(new Guid("51000000-0000-0000-0000-000000000002"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            EnteredOn = DateTimeOffset.Now.AddDays(-1),
            Notes = string.Empty,
        },
        new(new Guid("51000000-0000-0000-0000-000000000003"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetUsers.ElementAt(1),
            EnteredOn = DateTimeOffset.Now.AddDays(-2),
            Notes = "Deleted Action Item",
        },
    };

    private static IEnumerable<ActionItem>? _actionItems;

    public static IEnumerable<ActionItem> GetActionItems
    {
        get
        {
            if (_actionItems is not null) return _actionItems;
            _actionItems = ActionItemSeedItems.ToList();
            _actionItems.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000001");
            return _actionItems;
        }
    }

    public static void ClearData() => _actionItems = null;
}
