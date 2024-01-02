using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.SicCodes;
using Sbeap.Domain.Identity;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.AppServices.Customers;

public sealed class CustomerService(
    IMapper mapper,
    IUserService users,
    ICustomerRepository customers,
    ICustomerManager manager,
    IContactRepository contacts,
    ISicRepository sic)
    : ICustomerService
{
    // Customer read

    public async Task<IPaginatedResult<CustomerSearchResultDto>> SearchAsync(
        CustomerSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);

        var count = await customers.CountAsync(predicate, token);

        var list = count > 0
            ? mapper.Map<List<CustomerSearchResultDto>>(await customers.GetPagedListAsync(predicate, paging, token))
            : new List<CustomerSearchResultDto>();

        return new PaginatedResult<CustomerSearchResultDto>(list, count, paging);
    }

    public async Task<CustomerViewDto?> FindAsync(
        Guid id, bool includeDeletedCases = false, CancellationToken token = default)
    {
        var customer = await customers.FindIncludeAllAsync(id, includeDeletedCases, token);
        if (customer is null) return null;

        var view = mapper.Map<CustomerViewDto>(customer);
        return customer is { IsDeleted: true, DeletedById: not null }
            ? view with { DeletedBy = mapper.Map<StaffViewDto>(await users.FindUserAsync(customer.DeletedById)) }
            : view;
    }

    public async Task<CustomerSearchResultDto?> FindBasicInfoAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<CustomerSearchResultDto>(await customers.FindAsync(id, token));

    // Customer write

    public async Task<Guid> CreateAsync(CustomerCreateDto resource, CancellationToken token = default)
    {
        var user = await users.GetCurrentUserAsync();
        var customer = manager.Create(resource.Name, user?.Id);

        customer.Description = resource.Description;
        customer.SicCode = resource.SicCodeId is null ? null : await sic.GetAsync(resource.SicCodeId, token);
        customer.County = resource.County;
        customer.Website = resource.Website;
        customer.Location = resource.Location;
        customer.MailingAddress = resource.MailingAddress;

        await customers.InsertAsync(customer, autoSave: false, token: token);
        await CreateContactAsync(customer, resource.Contact, user, token);

        await customers.SaveChangesAsync(token);
        return customer.Id;
    }

    public async Task<CustomerUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<CustomerUpdateDto>(await customers.FindAsync(id, token));

    public async Task UpdateAsync(Guid id, CustomerUpdateDto resource, CancellationToken token = default)
    {
        var item = await customers.GetAsync(id, token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        item.Name = resource.Name;
        item.Description = resource.Description;
        item.SicCode = resource.SicCodeId is null ? null : await sic.GetAsync(resource.SicCodeId, token);
        item.County = resource.County;
        item.Location = resource.Location;
        item.MailingAddress = resource.MailingAddress;

        await customers.UpdateAsync(item, token: token);
    }

    public async Task DeleteAsync(Guid id, string? deleteComments, CancellationToken token = default)
    {
        var item = await customers.GetAsync(id, token);
        item.SetDeleted((await users.GetCurrentUserAsync())?.Id);
        item.DeleteComments = deleteComments;
        await customers.UpdateAsync(item, token: token);
    }

    public async Task RestoreAsync(Guid id, CancellationToken token = default)
    {
        var item = await customers.GetAsync(id, token);
        item.SetNotDeleted();
        await customers.UpdateAsync(item, token: token);
    }

    // Contacts

    public async Task<Guid> AddContactAsync(ContactCreateDto resource, CancellationToken token = default)
    {
        var customer = await customers.GetAsync(resource.CustomerId, token);
        var id = await CreateContactAsync(customer, resource, await users.GetCurrentUserAsync(), token);
        await contacts.SaveChangesAsync(token);
        return id;
    }

    private async Task<Guid> CreateContactAsync(
        Customer customer, ContactCreateDto resource, ApplicationUser? user, CancellationToken token = default)
    {
        if (resource.IsEmpty) return Guid.Empty;

        var contact = manager.CreateContact(customer, user?.Id);

        contact.Honorific = resource.Honorific;
        contact.GivenName = resource.GivenName;
        contact.FamilyName = resource.FamilyName;
        contact.Title = resource.Title;
        contact.Email = resource.Email;
        contact.Notes = resource.Notes;
        contact.Address = resource.Address;
        contact.EnteredBy = user;

        if (!resource.PhoneNumber.IsIncomplete)
        {
            contact.PhoneNumbers.Add(
                manager.CreatePhoneNumber(resource.PhoneNumber.Number, resource.PhoneNumber.Type.Value));
        }

        await contacts.InsertAsync(contact, autoSave: false, token: token);
        return contact.Id;
    }

    public async Task<ContactViewDto?> FindContactAsync(Guid contactId, CancellationToken token = default) =>
        mapper.Map<ContactViewDto>(await contacts.FindAsync(e => e.Id == contactId && !e.IsDeleted, token));

    public async Task<ContactUpdateDto?> FindContactForUpdateAsync(Guid contactId, CancellationToken token = default) =>
        mapper.Map<ContactUpdateDto>(await contacts.FindAsync(e => e.Id == contactId && !e.IsDeleted, token));

    public async Task UpdateContactAsync(Guid contactId, ContactUpdateDto resource, CancellationToken token = default)
    {
        var item = await contacts.GetAsync(contactId, token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        item.Honorific = resource.Honorific;
        item.GivenName = resource.GivenName;
        item.FamilyName = resource.FamilyName;
        item.Title = resource.Title;
        item.Email = resource.Email;
        item.Notes = resource.Notes;
        item.Address = resource.Address;

        await contacts.UpdateAsync(item, token: token);
    }

    public async Task DeleteContactAsync(Guid contactId, CancellationToken token = default)
    {
        var item = await contacts.GetAsync(contactId, token);
        item.SetDeleted((await users.GetCurrentUserAsync())?.Id);
        await contacts.UpdateAsync(item, token: token);
    }

    public async Task<PhoneNumber> AddPhoneNumberAsync(Guid contactId, PhoneNumberCreate resource,
        CancellationToken token = default)
    {
        var contact = await contacts.GetAsync(contactId, token);
        var phoneNumber = manager.CreatePhoneNumber(resource.Number!, resource.Type!.Value);
        contact.PhoneNumbers.Add(phoneNumber);
        await contacts.UpdateAsync(contact, token: token);
        return phoneNumber;
    }

    public async Task DeletePhoneNumberAsync(Guid contactId, int phoneNumberId, CancellationToken token = default)
    {
        var contact = await contacts.GetAsync(contactId, token);
        contact.PhoneNumbers.RemoveAll(p => p.Id == phoneNumberId);
        await contacts.UpdateAsync(contact, token: token);
    }

    public void Dispose()
    {
        customers.Dispose();
        contacts.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await customers.DisposeAsync();
        await contacts.DisposeAsync();
    }
}
