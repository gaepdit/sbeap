using JetBrains.Annotations;

namespace Sbeap.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static DevSettingsSection DevSettings { get; set; } = new();

    // PROD configuration settings
    private static readonly DevSettingsSection ProductionDefault = new()
    {
        UseDevSettings = false,
        UseInMemoryData = false,
        UseEfMigrations = true,
        UseAzureAd = true,
        LocalUserIsAuthenticated = false,
        LocalUserIsStaff = false,
        LocalUserIsAdmin = false,
        UseSecurityHeadersInDev = false,
    };

    // DEV configuration settings
    public class DevSettingsSection
    {
        /// <summary>
        /// Enable (`true`) or disable (`false`) the development settings.
        /// </summary>
        public bool UseDevSettings { get;  init; }

        /// <summary>
        /// Uses in-memory data when `true`. Connects to a SQL Server database when `false`.
        /// </summary>
        public bool UseInMemoryData { get;  init; }

        /// <summary>
        /// Uses Entity Framework migrations when `true`. When set to `false`, the database is deleted and
        /// recreated on each run. (Only applies if <see cref="UseInMemoryData"/> is `false`.)
        /// </summary>
        public bool UseEfMigrations { get;  init; }

        /// <summary>
        /// If `true`, the app must be registered in the Azure portal, and configuration settings added in the
        /// "AzureAd" settings section. If `false`, authentication is simulated using test user data.
        /// </summary>
        public bool UseAzureAd { get;  init; }

        /// <summary>
        /// Simulates a successful login with a test account when `true`. Simulates a failed login when `false`.
        /// (Only applies if <see cref="UseAzureAd"/> is `false`.)
        /// </summary>
        public bool LocalUserIsAuthenticated { get;  init; }

        /// <summary>
        /// Adds the Staff and Site Maintenance roles when `true` or no roles when `false`.
        /// (Only applies if <see cref="LocalUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public bool LocalUserIsStaff { get;  init; }

        /// <summary>
        /// Adds all App Roles to the logged in account when `true` or no roles when `false`. (Applies whether
        /// <see cref="UseAzureAd"/> is `true` or `false`.)
        /// </summary>
        public bool LocalUserIsAdmin { get;  init; }

        /// <summary>
        /// Sets whether to include HTTP security headers when running locally in the Development environment.
        /// </summary>
        public bool UseSecurityHeadersInDev { get;  init; }
    }
}
