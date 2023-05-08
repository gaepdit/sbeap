using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Offices;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.RegisterServices;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Cases/Action Items
        services.AddScoped<ICaseworkService, CaseworkService>();
        services.AddScoped<ICaseworkManager, CaseworkManager>();

        // Customers/Contacts
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerManager, CustomerManager>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();
    }
}
