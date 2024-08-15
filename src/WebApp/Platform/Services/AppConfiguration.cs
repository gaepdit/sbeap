using Sbeap.WebApp.Platform.Settings;
using System.Reflection;

namespace Sbeap.WebApp.Platform.Services;

public static class AppConfiguration
{
    public static void BindSettings(WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings))
            .Bind(ApplicationSettings.RaygunSettings);

        // App versioning
        var versionSegments = (Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "").Split('+');

        ApplicationSettings.RaygunSettings.InformationalVersion = versionSegments[0];

        // Dev settings
        var devConfig = builder.Configuration.GetSection(nameof(ApplicationSettings.DevSettings));
        var useDevConfig = devConfig.Exists() && Convert.ToBoolean(builder.Configuration["UseDevSettings"]);

        if (useDevConfig && devConfig.Exists())
            devConfig.Bind(ApplicationSettings.DevSettings);
        else
            ApplicationSettings.DevSettings = ApplicationSettings.ProductionDefault;
    }
}
