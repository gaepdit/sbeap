namespace Sbeap.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; }
    public string PluralName { get; private init; }
    public bool StartsWithVowelSound { get; private init; }

    private MaintenanceOption(string singularName, string pluralName, bool startsWithVowelSound = false)
    {
        SingularName = singularName;
        PluralName = pluralName;
        StartsWithVowelSound = startsWithVowelSound;
    }

    public static MaintenanceOption Office =>
        new(singularName: "Office", pluralName: "Offices", startsWithVowelSound: true);

    public static MaintenanceOption ActionItemType =>
        new(singularName: "Action Item Type", pluralName: "Action Item Types", startsWithVowelSound: true);

    public static MaintenanceOption Agency =>
        new(singularName: "Agency", pluralName: "Agencies", startsWithVowelSound: true);
}
