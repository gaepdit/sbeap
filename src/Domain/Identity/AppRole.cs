namespace Sbeap.Domain.Identity;

/// <summary>
/// User Roles available to the application for authorization.
/// </summary>
public static class RoleName
{
    // These are the strings that are stored in the database. Avoid modifying these once set!

    public const string Admin = nameof(Admin);
    public const string Staff = nameof(Staff);
    public const string SiteMaintenance = nameof(SiteMaintenance);
    public const string UserAdmin = nameof(UserAdmin);
}

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
/// </summary>
public class AppRole
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }

    private AppRole(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name, this);
    }

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// (This declaration must appear before the list of static instance types.)
    /// </summary>
    public static Dictionary<string, AppRole> AllRoles { get; } = new();

    /// <summary>
    /// Converts a list of role strings to a list of <see cref="AppRole"/> objects.
    /// </summary>
    /// <param name="roles">A list of role strings.</param>
    /// <returns>A list of AppRoles.</returns>
    public static IEnumerable<AppRole> RolesAsAppRoles(IEnumerable<string> roles)
    {
        var appRoles = new List<AppRole>();

        foreach (var role in roles)
            if (AllRoles.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }

    // These static Role objects are used for displaying role information in the UI.

    public static AppRole AdminRole { get; } = new(
        RoleName.Admin, "SBEAP Admin",
        "Can delete and restore customers and cases."
    );

    public static AppRole StaffRole { get; } = new(
        RoleName.Staff, "SBEAP Staff",
        "Can add, view, and edit customers and cases. Can add and remove contacts related " +
        "to customers. Can add and remove action items related to cases."
    );

    public static AppRole SiteMaintenanceRole { get; } = new(
        RoleName.SiteMaintenance, "Site Maintenance",
        "Can update values in lookup tables (drop-down lists)."
    );

    public static AppRole UserAdminRole { get; } = new(
        RoleName.UserAdmin, "User Account Admin",
        "Can register and edit all users and roles."
    );
}
