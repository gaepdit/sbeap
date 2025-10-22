using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.AuthenticationServices;

public static class PrincipalExtensions
{
    internal static bool IsAdmin(this IPrincipal user) => user.IsInRole(RoleName.Admin);
    internal static bool IsStaff(this IPrincipal user) => user.IsInRole(RoleName.Staff) || user.IsAdmin();
    internal static bool IsSiteMaintainer(this IPrincipal user) => user.IsInRole(RoleName.SiteMaintenance);
    internal static bool IsUserAdmin(this IPrincipal user) => user.IsInRole(RoleName.UserAdmin);
    internal static bool IsStaffOrMaintainer(this IPrincipal user) => user.IsStaff() || user.IsSiteMaintainer();
}
