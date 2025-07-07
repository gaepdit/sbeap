﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<IActionResult> OnGetAsync() => LogOutAndRedirectToIndex();

    public Task<IActionResult> OnPostAsync() => LogOutAndRedirectToIndex();

    private async Task<IActionResult> LogOutAndRedirectToIndex()
    {
        // If Azure AD is enabled, sign out all authentication schemes.
        if (AppSettings.DevSettings.UseAzureAd)
            return SignOut(new AuthenticationProperties { RedirectUri = "/Index" },
                IdentityConstants.ApplicationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);

        // If a local user is enabled instead, sign out locally and redirect to home page.
        await signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
