using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.TestData;

internal static class ActionItemTypeData
{
    private static IEnumerable<ActionItemType> ActionItemTypeSeedItems => new List<ActionItemType>
    {
        new(new Guid("20000000-0000-0000-0000-000000000001"), "CAP meetings") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000002"), "Compliance Assistance") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000003"), "Drafting Small Bus Impact Memos") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000004"), "Email Sent/Received"),
        new(new Guid("20000000-0000-0000-0000-000000000005"), "Mass Mailing") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000006"), "Meeting/Conferences Attended") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000007"), "Other"),
        new(new Guid("20000000-0000-0000-0000-000000000008"), "Permit Assistance"),
        new(new Guid("20000000-0000-0000-0000-000000000009"), "Phone Call Made/Received"),
        new(new Guid("20000000-0000-0000-0000-000000000010"), "Publication/Document Sent") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000011"), "Site Visit"),
        new(new Guid("20000000-0000-0000-0000-000000000012"), "Stakeholder meetings") { Active = false },
        new(new Guid("20000000-0000-0000-0000-000000000013"), "Workshops/Training Courses Conducted")
            { Active = false },
    };

    private static IEnumerable<ActionItemType>? _actionItemTypes;

    public static IEnumerable<ActionItemType> GetActionItemTypes
    {
        get
        {
            if (_actionItemTypes is not null) return _actionItemTypes;
            _actionItemTypes = ActionItemTypeSeedItems;
            return _actionItemTypes;
        }
    }

    public static void ClearData() => _actionItemTypes = null;
}
