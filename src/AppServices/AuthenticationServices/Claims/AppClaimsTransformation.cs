using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Identity;
using System.Security.Claims;

namespace Sbeap.AppServices.AuthenticationServices.Claims;

public static class AppClaimTypes
{
    public const string ActiveUser = nameof(ActiveUser);
    public const string OfficeId = nameof(OfficeId);
}

public class AppClaimsTransformation(UserManager<ApplicationUser> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var applicationUser = await userManager.GetUserAsync(principal).ConfigureAwait(false);

        foreach (var identity in principal.Identities)
            identity.TryRemoveClaim(identity.FindFirst(nameof(AppClaimTypes.ActiveUser)));

        var claimsIdentity = new ClaimsIdentity();
        AddNewClaim(claimsIdentity, principal, AppClaimTypes.ActiveUser, applicationUser?.Active.ToString());
        AddNewClaim(claimsIdentity, principal, AppClaimTypes.OfficeId, applicationUser?.Office?.Id.ToString());

        principal.AddIdentity(claimsIdentity);
        return principal;
    }

    private static void AddNewClaim(ClaimsIdentity claimsIdentity, ClaimsPrincipal principal,
        string type, string? value)
    {
        if (value != null && !principal.HasClaim(type, value)) claimsIdentity.AddClaim(new Claim(type, value));
    }
}
