using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Web;
using Okta.AspNetCore;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.AppServices.AuthorizationPolicies;

namespace Sbeap.WebApp.Platform.AppConfiguration;

public static class AuthenticationServices
{
    public static void ConfigureAuthentication(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        var authenticationBuilder = builder.Services
            .ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".Sbeap.Identity";
                options.Cookie.Path = configuration.GetValue<string>("CookiePath");
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        if (configuration.LoginProviderNames().Contains(LoginProviders.OktaScheme))
        {
            // Requires an Okta account
            authenticationBuilder.AddOktaMvc(authenticationScheme: LoginProviders.OktaScheme, new OktaMvcOptions
            {
                OktaDomain = configuration.GetValue<string>("Okta:OktaDomain"),
                AuthorizationServerId = configuration.GetValue<string>("Okta:AuthorizationServerId"),
                ClientId = configuration.GetValue<string>("Okta:ClientId"),
                ClientSecret = configuration.GetValue<string>("Okta:ClientSecret"),
                Scope = new List<string> { "openid", "profile", "email" },
            });
        }

        if (configuration.LoginProviderNames().Contains(LoginProviders.EntraIdScheme))
        {
            // Requires an Entra ID account
            authenticationBuilder.AddMicrosoftIdentityWebApp(configuration,
                openIdConnectScheme: LoginProviders.EntraIdScheme, cookieScheme: null);
            // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
        }

        builder.Services
            .AddAuthenticationAppServices()
            .AddAuthorizationPolicies()
            .AddAuthorization();
    }
}
