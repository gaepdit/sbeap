﻿using Microsoft.Extensions.Configuration;

namespace Sbeap.AppServices.AuthenticationServices;

public static class LoginProviderValidation
{
    private static IEnumerable<LoginProvider>? _loginProviders;
    private static IEnumerable<string>? _loginProviderNames;
    private const string EnabledLoginProviders = "EnabledLoginProviders";

    private static IEnumerable<LoginProvider> LoginProviders(this IConfiguration configuration)
    {
        _loginProviders ??= configuration.GetSection(EnabledLoginProviders).Get<LoginProvider[]>() ?? [];
        return _loginProviders;
    }

    public static IEnumerable<string> LoginProviderNames(this IConfiguration configuration)
    {
        _loginProviderNames ??= configuration.LoginProviders().Select(lp => lp.Name);
        return _loginProviderNames;
    }

    public static bool ValidateLoginProviderId(this IConfiguration configuration, string loginProvider,
        string identityProviderId)
    {
        if (string.IsNullOrEmpty(loginProvider) || string.IsNullOrEmpty(identityProviderId)) return false;
        return configuration.LoginProviders().Contains(new LoginProvider(loginProvider, identityProviderId));
    }

    public static bool ValidateLoginProvider(this IConfiguration configuration, string loginProvider) =>
        !string.IsNullOrEmpty(loginProvider) && configuration.LoginProviderNames().Contains(loginProvider);
}

public record LoginProvider(string Name, string Id);

public static class LoginProviders
{
    public const string OktaScheme = "Okta";
    public const string EntraIdScheme = "EntraId";
    public const string TestUserScheme = "TestUser";
}
