using JetBrains.Annotations;
using System.Reflection;

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
}
