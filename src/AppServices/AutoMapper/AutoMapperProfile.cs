using AutoMapper;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Action Items
        CreateMap<ActionItem, ActionItemViewDto>();
        CreateMap<ActionItem, ActionItemUpdateDto>();

        // Action Item Types
        CreateMap<ActionItemType, ActionItemTypeViewDto>();
        CreateMap<ActionItemType, ActionItemTypeUpdateDto>();

        // Agencies
        CreateMap<Agency, AgencyViewDto>();
        CreateMap<Agency, AgencyUpdateDto>();

        // Cases
        CreateMap<Casework, CaseworkViewDto>()
            .ForMember(e => e.DeletedBy, o => o.Ignore());
        CreateMap<Casework, CaseworkUpdateDto>();
        CreateMap<Casework, CaseworkSearchResultDto>();

        // Contacts
        CreateMap<Contact, ContactViewDto>();
        CreateMap<Contact, ContactUpdateDto>();

        // Customers
        CreateMap<Customer, CustomerViewDto>()
            .ForMember(e => e.DeletedBy, o => o.Ignore());
        CreateMap<Customer, CustomerUpdateDto>();
        CreateMap<Customer, CustomerSearchResultDto>();

        // Offices
        CreateMap<Office, OfficeViewDto>();
        CreateMap<Office, OfficeUpdateDto>();

        // Staff
        CreateMap<ApplicationUser, StaffViewDto>();
        CreateMap<ApplicationUser, StaffUpdateDto>();
        CreateMap<ApplicationUser, StaffSearchResultDto>();
    }
}
