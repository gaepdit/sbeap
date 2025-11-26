using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.AuthenticationServices;

public static class PrincipalExtensions
{
    extension(IPrincipal user)
    {
        internal bool IsAdmin() => user.IsInRole(RoleName.Admin);
        internal bool IsStaff() => user.IsInRole(RoleName.Staff) || user.IsAdmin();
        internal bool IsSiteMaintainer() => user.IsInRole(RoleName.SiteMaintenance);
        internal bool IsUserAdmin() => user.IsInRole(RoleName.UserAdmin);
        internal bool IsStaffOrMaintainer() => user.IsStaff() || user.IsSiteMaintainer();
    }
}
