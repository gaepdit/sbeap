﻿using System.Security.Claims;

namespace Sbeap.AppServices.AuthenticationServices.Claims;

public static class ClaimsPrincipalExtensions
{
    // Identity Provider claim types
    private const string IdentityProviderId = "idp";
    private const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

    public static string? GetIdentityProviderId(this ClaimsPrincipal principal) =>
        principal.GetClaimValue(IdentityProviderId, TenantId);

    public static string? GetAuthenticationMethod(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.AuthenticationMethod);

    public static string? GetEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email);

    public static string GetGivenName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

    public static string GetFamilyName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

    public static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

    private static string? GetClaimValue(this ClaimsPrincipal claimsPrincipal, params string[] claimNames) => claimNames
        .Select(claimsPrincipal.FindFirstValue)
        .FirstOrDefault(currentValue => !string.IsNullOrEmpty(currentValue));
}
