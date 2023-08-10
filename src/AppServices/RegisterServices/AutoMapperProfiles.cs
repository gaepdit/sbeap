using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.AutoMapper;

namespace Sbeap.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        // Add AutoMapper profiles
        services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
    }
}
