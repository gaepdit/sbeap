using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.AppServices.AuthenticationServices.Claims;
using Sbeap.Domain.Identity;

namespace Sbeap.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<SignOutResult> OnGetAsync() => SignOut();
    public Task<SignOutResult> OnPostAsync(string? returnUrl = null) => SignOut(returnUrl);

    private async Task<SignOutResult> SignOut(string? returnUrl = null)
    {
        var authenticationProperties = new AuthenticationProperties { RedirectUri = returnUrl ?? "../" };
        var userAuthenticationScheme = User.GetAuthenticationMethod();

        if (userAuthenticationScheme is null or LoginProviders.TestUserScheme)
        {
            await signInManager.SignOutAsync();
            return SignOut(authenticationProperties);
        }

        List<string> authenticationSchemes = [CookieAuthenticationDefaults.AuthenticationScheme];

        authenticationSchemes.AddRange([IdentityConstants.ApplicationScheme, userAuthenticationScheme]);

        return SignOut(authenticationProperties, authenticationSchemes.ToArray());
    }
}
