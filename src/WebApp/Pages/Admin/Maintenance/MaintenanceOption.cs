namespace Sbeap.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;

    private MaintenanceOption() { }

    public static MaintenanceOption Office =>
        new() { SingularName = "Office", PluralName = "Offices" };

    public static MaintenanceOption ActionItemType =>
        new() { SingularName = "Action Item Type", PluralName = "Action Item Types" };

    public static MaintenanceOption Agency =>
        new() { SingularName = "Agency", PluralName = "Agencies" };
}
