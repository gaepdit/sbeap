using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.SicCodes;
using Sbeap.AppServices.Staff;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.ServiceRegistration;

public static class AppServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services) => services

        // Action Item Types
        .AddScoped<IActionItemTypeService, ActionItemTypeService>()

        // Agencies
        .AddScoped<IAgencyService, AgencyService>()
        .AddScoped<IAgencyManager, AgencyManager>()

        // Action Item Types
        .AddScoped<IActionItemTypeService, ActionItemTypeService>()
        .AddScoped<IActionItemTypeManager, ActionItemTypeManager>()

        // Cases/Action Items
        .AddScoped<IActionItemService, ActionItemService>()
        .AddScoped<ICaseworkService, CaseworkService>()
        .AddScoped<ICaseworkManager, CaseworkManager>()

        // Customers/Contacts
        .AddScoped<ICustomerService, CustomerService>()
        .AddScoped<ICustomerManager, CustomerManager>()

        // Offices
        .AddScoped<IOfficeManager, OfficeManager>()
        .AddScoped<IOfficeService, OfficeService>()

        // SIC Codes
        .AddScoped<ISicService, SicService>()

        // Staff
        .AddScoped<IStaffService, StaffService>()

        // Validators
        .AddValidatorsFromAssemblyContaining(typeof(AppServiceRegistration));
}
