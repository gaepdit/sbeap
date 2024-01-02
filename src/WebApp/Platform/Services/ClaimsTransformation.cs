﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Sbeap.AppServices.Permissions;
using Sbeap.Domain.Identity;
using System.Security.Claims;

namespace Sbeap.WebApp.Platform.Services;

public class ClaimsTransformation(UserManager<ApplicationUser> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userIsActive = (await userManager.GetUserAsync(principal))?.Active ?? false;

        if (principal.HasClaim(nameof(Policies.ActiveUser), userIsActive.ToString()))
            return principal;

        foreach (var identity in principal.Identities)
            identity.TryRemoveClaim(identity.FindFirst(nameof(Policies.ActiveUser)));

        (principal.Identity as ClaimsIdentity)!
            .AddClaim(new Claim(nameof(Policies.ActiveUser), userIsActive.ToString()));

        return principal;
    }
}
