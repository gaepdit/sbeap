using Sbeap.Domain.Entities.Contacts;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalContactRepository()
    : BaseRepository<Contact, Guid>(ContactData.GetContacts(true)), IContactRepository
{
    // Local repository requires ID to be manually set.
    public int GetNextPhoneNumberId() => Items
        .SelectMany(e => e.PhoneNumbers)
        .Max(p => p.Id) + 1;
}
