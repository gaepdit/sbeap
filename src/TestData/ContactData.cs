using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.ValueObjects;
using Sbeap.TestData.Constants;
using Sbeap.TestData.Identity;

namespace Sbeap.TestData;

internal static class ContactData
{
    private static IEnumerable<Contact> ContactSeedItems(int seedPhoneIds) => new List<Contact>
    {
        new(new Guid("41000000-0000-0000-0000-000000000001"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            EnteredBy = UserData.GetUsers.ElementAt(1),
            EnteredOn = DateTimeOffset.Now.AddDays(-5),
            Honorific = "Mr.",
            GivenName = TextData.Word,
            FamilyName = TextData.AnotherWord,
            Title = TextData.ShortPhrase,
            Email = TextData.ValidEmail,
            Notes = TextData.MultipleParagraphs,
            Address = ValueObjectData.CompleteAddress,
            PhoneNumbers =
            {
                ValueObjectData.ValidPhoneNumber(seedPhoneIds * 91),
                ValueObjectData.AlternatePhoneNumber(seedPhoneIds * 92),
                ValueObjectData.AdditionalPhoneNumber(seedPhoneIds * 93),
            },
        },
        new(new Guid("41000000-0000-0000-0000-000000000002"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            EnteredBy = UserData.GetUsers.ElementAt(0),
            EnteredOn = DateTimeOffset.Now.AddDays(-4),
            Honorific = string.Empty,
            GivenName = string.Empty,
            FamilyName = string.Empty,
            Title = TextData.AnotherShortPhrase,
            Email = string.Empty,
            Notes = string.Empty,
            Address = IncompleteAddress.EmptyAddress,
        },
        new(new Guid("41000000-0000-0000-0000-000000000003"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            EnteredBy = UserData.GetUsers.ElementAt(3),
            EnteredOn = DateTimeOffset.Now.AddDays(-3),
            Honorific = "Ms.",
            GivenName = "Deleted",
            FamilyName = "Contact",
            Title = TextData.Phrase,
            Email = TextData.ValidEmail,
            Notes = TextData.Paragraph,
            Address = ValueObjectData.LessCompleteAddress,
            PhoneNumbers = { ValueObjectData.ValidPhoneNumber(seedPhoneIds * 94) },
        },
        new(new Guid("41000000-0000-0000-0000-000000000004"),
            CustomerData.GetCustomers.ElementAt(2))
        {
            EnteredBy = UserData.GetUsers.ElementAt(1),
            EnteredOn = DateTimeOffset.Now.AddDays(-2),
            Honorific = "Mx.",
            GivenName = TextData.AnotherWord,
            FamilyName = TextData.EmojiWord,
            Title = TextData.Phrase,
            Email = TextData.ValidEmail,
            Notes = TextData.Paragraph,
            Address = ValueObjectData.LessCompleteAddress,
            PhoneNumbers = { ValueObjectData.ValidPhoneNumber(seedPhoneIds * 95) },
        },
    };

    private static IEnumerable<Contact>? _contacts;

    public static IEnumerable<Contact> GetContacts(bool seedPhoneIds)
    {
        if (_contacts is not null) return _contacts;

        // When `seedPhoneIds` is true, Phone Number IDs will be seeded.
        // When `seedPhoneIds` is false, all Phone Number IDs will be set to zero so that EF can auto-create them.
        _contacts = ContactSeedItems(seedPhoneIds ? 1 : 0).ToList();

        _contacts.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000001");
        return _contacts;
    }

    public static void ClearData() => _contacts = null;
}
