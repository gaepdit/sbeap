using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.AppServices.Customers;

public interface ICustomerService : IDisposable
{
    // Customer read
    Task<IPaginatedResult<CustomerSearchResultDto>> SearchAsync(
        CustomerSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<CustomerViewDto?> FindAsync(Guid id, CancellationToken token = default);

    // Customer write
    Task<Guid> CreateAsync(CustomerCreateDto resource, CancellationToken token = default);
    Task<CustomerUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(CustomerUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid id, CancellationToken token = default);

    // Contacts
    Task AddContactAsync(Customer customer, ContactCreateDto resource, CancellationToken token = default);
    Task<ContactUpdateDto?> FindContactForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateContactAsync(ContactUpdateDto resource, CancellationToken token = default);
    Task DeleteContactAsync(Guid contactId, CancellationToken token = default);
    Task AddPhoneNumberAsync(Guid contactId, PhoneNumberCreateDto resource, CancellationToken token = default);
    Task DeletePhoneNumberAsync(Guid phoneNumberId, CancellationToken token = default);
}
