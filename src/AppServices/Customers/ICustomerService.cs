using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.AppServices.Customers;

public interface ICustomerService : IDisposable
{
    // Customer read
    Task<IPaginatedResult<CustomerSearchResultDto>> SearchAsync(
        CustomerSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<CustomerViewDto?> FindAsync(Guid id, bool includeDeletedCases = false, CancellationToken token = default);
    Task<CustomerSearchResultDto?> FindBasicInfoAsync(Guid id, CancellationToken token = default);

    // Customer write
    Task<Guid> CreateAsync(CustomerCreateDto resource, CancellationToken token = default);
    Task<CustomerUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(CustomerUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid id, string? deleteComments, CancellationToken token = default);
    Task RestoreAsync(Guid id, CancellationToken token = default);

    // Contacts
    Task<Guid> AddContactAsync(ContactCreateDto resource, CancellationToken token = default);
    Task<ContactUpdateDto?> FindContactForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateContactAsync(ContactUpdateDto resource, CancellationToken token = default);
    Task DeleteContactAsync(Guid contactId, CancellationToken token = default);
    Task AddPhoneNumberAsync(Guid contactId, PhoneNumber resource, CancellationToken token = default);
    Task DeletePhoneNumberAsync(Guid contactId, PhoneNumber resource, CancellationToken token = default);
}
