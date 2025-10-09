using Microsoft.Identity.Web;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.Services;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var authenticationBuilder = services
            .ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".Sbeap.Identity";
                options.Cookie.Path = "/sbeap";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddAuthentication();

        // An Azure AD app must be registered and configured in the settings file.
        if (AppSettings.DevSettings.UseAzureAd)
            authenticationBuilder.AddMicrosoftIdentityWebApp(configuration, cookieScheme: null);
        // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
    }
}
