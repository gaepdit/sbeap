using JetBrains.Annotations;

namespace Sbeap.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string? Version { get; private set; }
    public static Raygun RaygunSettings { get; } = new();
    public static string? OrgNotificationsApiUrl { get; private set; }

    public class Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    // Dev-related settings
    public static bool TestUserEnabled => DevSettings is { UseDevSettings: true, EnableTestUser: true };
    public static bool UseSecurityHeaders => DevSettings is not { UseDevSettings: true, EnableSecurityHeaders: false };
}
