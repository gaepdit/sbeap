using Sbeap.Domain.Entities.Contacts;

namespace Sbeap.EfRepository.Repositories;

public sealed class ContactRepository(AppDbContext context)
    : BaseRepository<Contact, Guid, AppDbContext>(context), IContactRepository
{
    // EF will set the ID automatically.
    public int GetNextPhoneNumberId() => 0;
}
