using JetBrains.Annotations;

namespace Sbeap.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string? Version { get; private set; }
    public static string? SimpleVersion => Version?.Split('+')[0];

    public static string Env { get; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown";
    public static string ShortEnv => Env switch { "Production" => "prod", "Staging" => "uat", _ => "dev" };

    public static Raygun RaygunSettings { get; } = new();
    public static DataDog DataDogSettings { get; } = new();
    public static string? OrgNotificationsApiUrl { get; private set; }

    public class Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    public record DataDog
    {
        public string ClientToken { get; [UsedImplicitly] init; }
        public string ApplicationId { get; [UsedImplicitly] init; }
    }

    // Dev-related settings
    public static bool TestUserEnabled => DevSettings is { UseDevSettings: true, EnableTestUser: true };
    public static bool UseSecurityHeaders => DevSettings is not { UseDevSettings: true, EnableSecurityHeaders: false };
}
