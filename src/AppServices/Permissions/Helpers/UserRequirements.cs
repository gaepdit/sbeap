using Sbeap.Domain.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Sbeap.AppServices.Permissions.Helpers;

public static class UserRequirements
{
    internal static bool IsActive(this ClaimsPrincipal user) =>
        user.HasClaim(claim => claim.Type == nameof(Policies.ActiveUser) && claim.Value == true.ToString());

    internal static bool IsAdmin(this IPrincipal user) => user.IsInRole(RoleName.Admin);
    internal static bool IsStaff(this IPrincipal user) => user.IsInRole(RoleName.Staff) || user.IsAdmin();
    internal static bool IsSiteMaintainer(this IPrincipal user) => user.IsInRole(RoleName.SiteMaintenance);
    internal static bool IsUserAdmin(this IPrincipal user) => user.IsInRole(RoleName.UserAdmin);
    internal static bool IsStaffOrMaintainer(this IPrincipal user) => user.IsStaff() || user.IsSiteMaintainer();
}
