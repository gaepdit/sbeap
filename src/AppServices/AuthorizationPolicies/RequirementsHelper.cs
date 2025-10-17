using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Sbeap.AppServices.AuthorizationPolicies;

public static class RequirementsHelper
{
    public static async Task<Dictionary<IAuthorizationRequirement, bool>> SetPermissions(
        this IAuthorizationService authorization, IEnumerable<IAuthorizationRequirement> requirements,
        ClaimsPrincipal user, object? resource)
    {
        var userCan = new Dictionary<IAuthorizationRequirement, bool>();

        foreach (var requirement in requirements)
        {
            userCan[requirement] = resource != null && (await authorization.AuthorizeAsync(user, resource, requirement)
                .ConfigureAwait(false)).Succeeded;
        }

        return userCan;
    }
}
