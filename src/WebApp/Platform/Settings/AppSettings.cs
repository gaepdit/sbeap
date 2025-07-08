using JetBrains.Annotations;
using System.Reflection;

namespace Sbeap.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string? Version { get; private set; }
    public static Raygun RaygunSettings { get; } = new();

    public class Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    public static WebApplicationBuilder BindAppSettings(this WebApplicationBuilder builder)
    {
        Version = GetVersion();

        builder.Configuration.GetSection(nameof(RaygunSettings)).Bind(RaygunSettings);

        // Dev settings
        var devConfig = builder.Configuration.GetSection(nameof(DevSettings));
        var useDevConfig = builder.Environment.IsDevelopment() && devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(DevSettings.UseDevSettings)]);

        if (useDevConfig) devConfig.Bind(DevSettings);
        else DevSettings = ProductionDefault;

        return builder;
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
