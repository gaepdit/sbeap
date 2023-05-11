using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.AppServices.Customers;

public sealed class CustomerService : ICustomerService
{
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    private readonly IStaffService _staff;
    private readonly ICustomerRepository _customers;
    private readonly ICustomerManager _manager;
    private readonly IContactRepository _contacts;

    public CustomerService(
        IMapper mapper, IUserService users, IStaffService staff, ICustomerRepository customers,
        ICustomerManager manager, IContactRepository contacts)
    {
        _mapper = mapper;
        _users = users;
        _customers = customers;
        _manager = manager;
        _contacts = contacts;
        _staff = staff;
    }

    // Customer read

    public async Task<IPaginatedResult<CustomerSearchResultDto>> SearchAsync(
        CustomerSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);

        var count = await _customers.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<CustomerSearchResultDto>>(await _customers.GetPagedListAsync(predicate, paging, token))
            : new List<CustomerSearchResultDto>();

        return new PaginatedResult<CustomerSearchResultDto>(list, count, paging);
    }

    public async Task<CustomerViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var customer = await _customers.FindIncludeAllAsync(id, token);
        if (customer is null) return null;

        var view = _mapper.Map<CustomerViewDto>(customer);
        if (customer is { IsDeleted: true, DeletedById: not null })
            view.DeletedBy = await _staff.FindAsync(customer.DeletedById);

        return view;
    }

    // Customer write

    public async Task<Guid> CreateAsync(CustomerCreateDto resource, CancellationToken token = default)
    {
        var item = _manager.Create(resource.Name);
        item.SetCreator((await _users.GetCurrentUserAsync())?.Id);
        item.Description = resource.Description;
        item.County = resource.County;
        item.Location = resource.Location;
        item.MailingAddress = resource.MailingAddress;

        await _customers.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<CustomerUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<CustomerUpdateDto>(await _customers.FindAsync(id, token));

    public async Task UpdateAsync(CustomerUpdateDto resource, CancellationToken token = default)
    {
        var item = await _customers.GetAsync(resource.Id, token);

        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);
        item.Name = resource.Name;
        item.Description = resource.Description;
        item.County = resource.County;
        item.Location = resource.Location;
        item.MailingAddress = resource.MailingAddress;

        await _customers.UpdateAsync(item, token: token);
    }

    public async Task DeleteAsync(Guid id, string comments, CancellationToken token = default)
    {
        var item = await _customers.GetAsync(id, token);
        item.SetDeleted((await _users.GetCurrentUserAsync())?.Id);
        item.DeleteComments = comments;
        await _customers.UpdateAsync(item, token: token);
    }

    // Contacts

    public async Task AddContactAsync(ContactCreateDto resource, CancellationToken token = default)
    {
        var customer = await _customers.GetAsync(resource.CustomerId, token);
        var item = _manager.CreateContact(customer);

        item.SetCreator((await _users.GetCurrentUserAsync())?.Id);
        item.Honorific = resource.Honorific;
        item.GivenName = resource.GivenName;
        item.FamilyName = resource.FamilyName;
        item.Title = resource.Title;
        item.Email = resource.Email;
        item.Notes = resource.Notes;
        item.Address = resource.Address;
        item.PhoneNumbers.Add(resource.PhoneNumber);

        await _contacts.InsertAsync(item, token: token);
    }

    public async Task<ContactUpdateDto?> FindContactForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<ContactUpdateDto>(await _contacts.FindAsync(id, token));

    public async Task UpdateContactAsync(ContactUpdateDto resource, CancellationToken token = default)
    {
        var item = await _contacts.GetAsync(resource.Id, token);

        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);
        item.Honorific = resource.Honorific;
        item.GivenName = resource.GivenName;
        item.FamilyName = resource.FamilyName;
        item.Title = resource.Title;
        item.Email = resource.Email;
        item.Notes = resource.Notes;
        item.Address = resource.Address;

        await _contacts.UpdateAsync(item, token: token);
    }

    public async Task DeleteContactAsync(Guid contactId, CancellationToken token = default)
    {
        var item = await _contacts.GetAsync(contactId, token);
        item.SetDeleted((await _users.GetCurrentUserAsync())?.Id);
        await _contacts.UpdateAsync(item, token: token);
    }

    public async Task AddPhoneNumberAsync(Guid contactId, PhoneNumber resource,
        CancellationToken token = default)
    {
        var item = await _contacts.GetAsync(contactId, token);
        item.PhoneNumbers.Add(resource);
        await _contacts.UpdateAsync(item, token: token);
    }

    public async Task DeletePhoneNumberAsync(Guid contactId, PhoneNumber resource, CancellationToken token = default)
    {
        var item = await _contacts.GetAsync(contactId, token);
        item.PhoneNumbers.Remove(resource);
        await _contacts.UpdateAsync(item, token: token);
    }

    public void Dispose()
    {
        _customers.Dispose();
        _contacts.Dispose();
    }
}
