using Sbeap.Domain.Entities.Contacts;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalContactRepository : BaseRepository<Contact, Guid>, IContactRepository
{
    public LocalContactRepository() : base(ContactData.GetContacts(true)) { }

    // Local repository requires ID to be manually set.
    public int GetNextPhoneNumberId() => Items
        .SelectMany(e => e.PhoneNumbers)
        .Max(p => p.Id) + 1;
}
