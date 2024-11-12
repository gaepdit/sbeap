using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.Logging;
using Sbeap.WebApp.Platform.PageModelHelpers;
using Sbeap.WebApp.Platform.Settings;
using System.Security.Claims;

namespace Sbeap.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    IStaffService staffService,
    ILogger<ExternalLoginModel> logger)
    : PageModel
{
    // Properties
    public ApplicationUser? DisplayFailedUser { get; private set; }
    public string? ReturnUrl { get; private set; }

    // Methods

    // Don't call the page directly
    public IActionResult OnGet() => RedirectToPage("./Login");

    // This Post method is called from the Login page
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        // Use AzureAD authentication if enabled; otherwise, sign in as local user.
        if (!ApplicationSettings.DevSettings.UseAzureAd) return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
        const string provider = OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    private async Task<IActionResult> SignInAsLocalUser()
    {
        logger.LogInformation(
            "Local user signin attempted with settings {LocalUserIsAuthenticated}, {LocalUserIsAdmin}, and {LocalUserIsStaff}",
            ApplicationSettings.DevSettings.LocalUserIsAuthenticated.ToString(),
            ApplicationSettings.DevSettings.LocalUserIsAdmin.ToString(),
            ApplicationSettings.DevSettings.LocalUserIsStaff.ToString());
        if (!ApplicationSettings.DevSettings.LocalUserIsAuthenticated) return Forbid();

        StaffSearchDto search;

        if (ApplicationSettings.DevSettings.LocalUserIsAdmin)
            search = new StaffSearchDto(SortBy.NameAsc, "Admin", null, null, null, null);
        else if (ApplicationSettings.DevSettings.LocalUserIsStaff)
            search = new StaffSearchDto(SortBy.NameAsc, "General", null, null, null, null);
        else
            search = new StaffSearchDto(SortBy.NameAsc, "Limited", null, null, null, null);

        var staffId = (await staffService.GetListAsync(search))[0].Id;

        var user = await userManager.FindByIdAsync(staffId);
        logger.LogInformation("Local user with ID {StaffId} signed in", staffId);

        user!.MostRecentLogin = DateTimeOffset.Now;
        await userManager.UpdateAsync(user);
        await signInManager.SignInAsync(user, false);

        return LocalRedirectOrHome();
    }

    // This callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl;

        // Handle errors returned from the external provider.
        if (remoteError is not null)
            return RedirectToLoginPageWithError($"Error from work account provider: {remoteError}");

        // Get information about the user from the external provider.
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo?.Principal is null)
            return RedirectToLoginPageWithError("Error loading work account information.");

        var userTenant = externalLoginInfo.Principal.GetTenantId();
        var userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (userEmail is null || userTenant is null)
            return RedirectToLoginPageWithError("Error loading detailed work account information.");

        if (!userEmail.IsValidEmailDomain())
        {
            logger.LogWarning("User {Email} with invalid email domain attempted signin", userEmail.MaskEmail());
            return RedirectToPage("./Unavailable");
        }

        logger.LogInformation("User {UserName} in tenant {TenantID} successfully authenticated", userEmail.MaskEmail(),
            userTenant);

        // Determine if a user account already exists with the Object ID.
        // If not, then determine if a user account already exists with the given username.
        var user = ApplicationSettings.DevSettings.UseInMemoryData
            ? await userManager.FindByNameAsync(userEmail)
            : await userManager.Users.SingleOrDefaultAsync(u =>
                  u.AzureAdObjectId == externalLoginInfo.Principal.GetObjectId()) ??
              await userManager.FindByNameAsync(userEmail);

        // If the user does not have a local account yet, then create one and sign in.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo);

        // If user has been marked as inactive, don't sign in.
        if (!user.Active)
        {
            logger.LogWarning("Inactive user {Email} attempted signin", userEmail.MaskEmail());
            return RedirectToPage("./Unavailable");
        }

        // Try to sign in the user locally with the external provider key.
        var signInResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("./Unavailable");
        }

        if (signInResult.Succeeded)
            return await RefreshUserInfoAndSignInAsync(user, externalLoginInfo);

        // If ExternalLoginInfo successfully returned from external provider, and the user exists, but
        // ExternalLoginSignInAsync failed, then add the external provider info to the user and sign in.
        return await AddLoginProviderAndSignInAsync(user, externalLoginInfo);
    }

    // Redirect to Login page with error message.
    private RedirectToPageResult RedirectToLoginPageWithError(string message)
    {
        logger.LogWarning("External login error: {Message}", message);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, message);
        return RedirectToPage("./Login", new { ReturnUrl });
    }

    // Create a new user account and sign in.
    private async Task<IActionResult> CreateUserAndSignInAsync(ExternalLoginInfo info)
    {
        var user = new ApplicationUser
        {
            UserName = info.Principal.GetDisplayName(),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "",
            FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "",
            AzureAdObjectId = info.Principal.GetObjectId(),
            AccountCreatedAt = DateTimeOffset.Now,
            MostRecentLogin = DateTimeOffset.Now,
        };

        // Create the user in the backing store.
        var createUserResult = await userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            logger.LogWarning("Failed to create new user {UserName}", user.Email.MaskEmail());
            return await FailedLogin(createUserResult, user);
        }

        logger.LogInformation("Created new user {Email} with object ID {ObjectId}",
            user.Email.MaskEmail(), user.AzureAdObjectId);

        // Add new user to application Roles if seeded in app settings or local admin user setting is enabled.
        var seedAdminUsers = configuration.GetSection("SeedAdminUsers").Get<string[]>();
        if (ApplicationSettings.DevSettings.LocalUserIsStaff)
        {
            logger.LogInformation("Seeding staff role for new user {Email}", user.Email.MaskEmail());
            await userManager.AddToRoleAsync(user, RoleName.Staff);
        }

        if (ApplicationSettings.DevSettings.LocalUserIsAdmin ||
            (seedAdminUsers != null && seedAdminUsers.Contains(user.Email, StringComparer.InvariantCultureIgnoreCase)))
        {
            logger.LogInformation("Seeding all roles for new user {UserName}", user.UserName);
            foreach (var role in AppRole.AllRoles) await userManager.AddToRoleAsync(user, role.Key);
        }

        // Add the external provider info to the user and sign in.
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            "Your account has successfully been created. Select “Edit Profile” to update your info.");
        return await AddLoginProviderAndSignInAsync(user, info, true);
    }

    // Update local store with from external provider. 
    private async Task<IActionResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        logger.LogInformation("Existing user {Email} logged in with {LoginProvider} provider",
            user.Email.MaskEmail(), info.LoginProvider);

        var previousValues = new ApplicationUser
        {
            UserName = user.UserName,
            Email = user.Email,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
        };

        user.UserName = info.Principal.GetDisplayName();
        user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
        user.GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
        user.FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;
        user.MostRecentLogin = DateTimeOffset.Now;

        if (user.UserName != previousValues.UserName || user.Email != previousValues.Email ||
            user.GivenName != previousValues.GivenName || user.FamilyName != previousValues.FamilyName)
        {
            user.AccountUpdatedAt = DateTimeOffset.Now;
        }

        await userManager.UpdateAsync(user);
        await signInManager.RefreshSignInAsync(user);
        return LocalRedirectOrHome();
    }

    // Add external login provider to user account and sign in user.
    private async Task<IActionResult> AddLoginProviderAndSignInAsync(
        ApplicationUser user, ExternalLoginInfo info, bool redirectToAccount = false)
    {
        var addLoginResult = await userManager.AddLoginAsync(user, info);

        if (!addLoginResult.Succeeded)
        {
            logger.LogWarning("Failed to add login provider {LoginProvider} for user {Email}",
                info.LoginProvider, user.Email.MaskEmail());
            return await FailedLogin(addLoginResult, user);
        }

        user.AzureAdObjectId ??= info.Principal.GetObjectId();
        user.MostRecentLogin = DateTimeOffset.Now;
        await userManager.UpdateAsync(user);

        logger.LogInformation("Login provider {LoginProvider} added for user {Email} with object ID {ObjectId}",
            info.LoginProvider, user.Email.MaskEmail(), user.AzureAdObjectId);

        // Include the access token in the properties.
        var props = new AuthenticationProperties();
        if (info.AuthenticationTokens is not null) props.StoreTokens(info.AuthenticationTokens);
        props.IsPersistent = true;

        await signInManager.SignInAsync(user, props, info.LoginProvider);
        // If new user, redirect to Account page
        return redirectToAccount ? RedirectToPage("./Index") : LocalRedirectOrHome();
    }

    // Add error info and return this Page.
    private async Task<PageResult> FailedLogin(IdentityResult result, ApplicationUser user)
    {
        DisplayFailedUser = user;
        await signInManager.SignOutAsync();
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }

    private IActionResult LocalRedirectOrHome() =>
        ReturnUrl is null ? RedirectToPage("/Index") : LocalRedirect(ReturnUrl);
}
