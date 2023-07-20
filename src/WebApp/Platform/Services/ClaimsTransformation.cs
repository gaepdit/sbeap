using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Sbeap.AppServices.Permissions;
using Sbeap.Domain.Identity;
using System.Security.Claims;

namespace Sbeap.WebApp.Platform.Services;

public class ClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ClaimsTransformation(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userIsActive = (await _userManager.GetUserAsync(principal))!.Active;

        if (principal.HasClaim(PolicyName.ActiveUser, userIsActive.ToString()))
            return principal;

        foreach (var identity in principal.Identities)
            identity.TryRemoveClaim(identity.FindFirst(PolicyName.ActiveUser));

        (principal.Identity as ClaimsIdentity)!.AddClaim(new Claim(PolicyName.ActiveUser, userIsActive.ToString()));

        return principal;
    }
}
