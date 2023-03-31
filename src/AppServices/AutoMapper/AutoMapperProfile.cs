using AutoMapper;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Staff;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Office, OfficeViewDto>().ReverseMap();
        CreateMap<Office, OfficeUpdateDto>();

        CreateMap<ApplicationUser, StaffViewDto>().ReverseMap();
        CreateMap<ApplicationUser, StaffUpdateDto>();
    }
}
