using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.AppConfiguration;

internal static class WebOptimizer
{
    public static IServiceCollection AddWebOptimizer(this IServiceCollection services)
    {
        if (AppSettings.DevSettings.UseDevSettings)
        {
            services.AddWebOptimizer(
                minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizer,
                minifyCss: AppSettings.DevSettings.EnableWebOptimizer);
        }
        else
        {
            services.AddWebOptimizer(minifyJavaScript: true, minifyCss: true);
        }

        return services;
    }
}
