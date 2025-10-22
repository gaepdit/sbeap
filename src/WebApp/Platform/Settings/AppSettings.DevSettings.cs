using JetBrains.Annotations;

namespace Sbeap.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // DEV configuration settings
    public static DevSettingsSection DevSettings { get; private set; } = new();

    private static void DisableDevSettings() => DevSettings = new DevSettingsSection { UseDevSettings = false };

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record DevSettingsSection
    {
        /// <summary>
        /// Enable (`true`) or disable (`false`) the development settings.
        /// </summary>
        public bool UseDevSettings { get; init; }

        /// <summary>
        /// Build a SQL Server database (`true`) or use an in-memory data store (`false`).
        /// </summary>
        public bool BuildDatabase { get; init; }

        /// <summary>
        /// Create a database using Entity Framework migrations (`true`) or solely based on the `DbContext` (`false`).
        /// (Only applies if <see cref="BuildDatabase"/> is `true`.)
        /// </summary>
        public bool UseEfMigrations { get; init; }

        /// <summary>
        /// Enable a test user for development (`true`) or disable (`false`).
        /// </summary>
        public bool EnableTestUser { get; init; }

        /// <summary>
        /// Simulate a successful login with a test account (`true`) or simulate a failed login (`false`).
        /// (Only applies if <see cref="EnableTestUser"/> is `false`.)
        /// </summary>
        public bool TestUserIsAuthenticated { get; init; }

        /// <summary>
        /// Add listed Roles to the test user account.
        /// (Only applies if <see cref="EnableTestUser"/> is `false` and <see cref="TestUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public string[] TestUserRoles { get; init; } = [];

        /// <summary>
        /// Include HTTP security headers when running in a Development environment (`true`).
        /// </summary>
        public bool EnableSecurityHeaders { get; init; }

        /// <summary>
        /// Use WebOptimizer to bundle and minify CSS and JS files (`true`).
        /// </summary>
        public bool EnableWebOptimizer { get; init; }
    }

    private static IHostApplicationBuilder BindDevAppSettings(this IHostApplicationBuilder builder)
    {
        // Dev settings should only be used in the development or staging environment and when explicitly enabled.
        var devConfig = builder.Configuration.GetSection(nameof(DevSettings));
        var useDevConfig = !builder.Environment.IsProduction() && devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(DevSettings.UseDevSettings)]);

        if (useDevConfig) devConfig.Bind(DevSettings);
        else DisableDevSettings();

        return builder;
    }
}
