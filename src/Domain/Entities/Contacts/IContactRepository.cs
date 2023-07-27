namespace Sbeap.Domain.Entities.Contacts;

public interface IContactRepository : IRepository<Contact, Guid>
{
    // Should return the next available Phone Number ID if the repository requires it for adding new entities (local repository).
    // Should return null if the repository creates a new ID on insert (Entity Framework).
    public int GetNextPhoneNumberId();
}
