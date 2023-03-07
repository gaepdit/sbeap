using Microsoft.Identity.Web;
using MyAppRoot.WebApp.Platform.Settings;

namespace MyAppRoot.WebApp.Platform.Services;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var authenticationBuilder = services.AddAuthentication();

        // An Azure AD app must be registered and configured in the settings file.
        if (ApplicationSettings.DevSettings.UseAzureAd)
            authenticationBuilder.AddMicrosoftIdentityWebApp(configuration, cookieScheme: null);
        // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
    }
}
