using Sbeap.Domain.Entities.Contacts;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalContactRepository : BaseRepository<Contact, Guid>, IContactRepository
{
    public LocalContactRepository() : base(ContactData.GetContacts) { }
}
