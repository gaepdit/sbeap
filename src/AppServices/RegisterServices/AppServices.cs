using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Offices;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.RegisterServices;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();
    }
}
