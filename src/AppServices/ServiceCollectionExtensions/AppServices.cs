using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.AutoMapper;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Staff;
using Sbeap.Domain.Offices;

namespace Sbeap.AppServices.ServiceCollectionExtensions;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeAppService, OfficeAppService>();
        services.AddScoped<IValidator<OfficeUpdateDto>, OfficeUpdateValidator>();
        services.AddScoped<IValidator<OfficeCreateDto>, OfficeCreateValidator>();

        // Staff
        services.AddScoped<IValidator<StaffUpdateDto>, StaffUpdateValidator>();
    }
}
