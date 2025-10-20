using System.Reflection;

namespace Sbeap.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static IHostApplicationBuilder BindAppSettings(this IHostApplicationBuilder builder)
    {
        Version = GetVersion();

        builder.Configuration.GetSection(nameof(RaygunSettings)).Bind(RaygunSettings);
        OrgNotificationsApiUrl = builder.Configuration.GetValue<string>(nameof(OrgNotificationsApiUrl));

        return builder.BindDevAppSettings();
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
