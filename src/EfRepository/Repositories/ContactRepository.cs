using Sbeap.Domain.Entities.Contacts;

namespace Sbeap.EfRepository.Repositories;

public sealed class ContactRepository : BaseRepository<Contact, Guid>, IContactRepository
{
    public ContactRepository(DbContext context) : base(context) { }

    // EF will set the ID automatically.
    public int GetNextPhoneNumberId() => 0;
}
