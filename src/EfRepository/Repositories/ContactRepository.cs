﻿using Sbeap.Domain.Entities.Contacts;
using Sbeap.EfRepository.Contexts;

namespace Sbeap.EfRepository.Repositories;

public sealed class ContactRepository : BaseRepository<Contact, Guid>, IContactRepository
{
    public ContactRepository(AppDbContext context) : base(context) { }
}