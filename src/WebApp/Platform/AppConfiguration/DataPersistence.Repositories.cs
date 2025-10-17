using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Entities.SicCodes;
using Sbeap.EfRepository.Repositories;
using Sbeap.LocalRepository.Repositories;

namespace Sbeap.WebApp.Platform.AppConfiguration;

internal static partial class DataPersistence
{
    private static void AddEntityFrameworkRepositories(this IServiceCollection services) => services
        .AddScoped<IActionItemRepository, ActionItemRepository>()
        .AddScoped<IActionItemTypeRepository, ActionItemTypeRepository>()
        .AddScoped<IAgencyRepository, AgencyRepository>()
        .AddScoped<ICaseworkRepository, CaseworkRepository>()
        .AddScoped<IContactRepository, ContactRepository>()
        .AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped<IOfficeRepository, OfficeRepository>()
        .AddScoped<ISicRepository, SicRepository>();

    private static void AddInMemoryRepositories(this IServiceCollection services) => services
        .AddSingleton<IActionItemRepository, LocalActionItemRepository>()
        .AddSingleton<IActionItemTypeRepository, LocalActionItemTypeRepository>()
        .AddSingleton<IAgencyRepository, LocalAgencyRepository>()
        .AddSingleton<ICaseworkRepository, LocalCaseworkRepository>()
        .AddSingleton<IContactRepository, LocalContactRepository>()
        .AddSingleton<ICustomerRepository, LocalCustomerRepository>()
        .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
        .AddSingleton<ISicRepository, LocalSicRepository>();
}
