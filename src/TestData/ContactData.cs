using Sbeap.Domain.Entities.Contacts;
using Sbeap.TestData.Constants;

namespace Sbeap.TestData;

internal static class ContactData
{
    private static IEnumerable<Contact> ContactSeedItems => new List<Contact>
    {
        new(new Guid("40000000-0000-0000-0000-000000000001"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            Honorific = "M.",
            GivenName = TextData.Word,
            FamilyName = TextData.AnotherWord,
            Title = TextData.ShortPhrase,
            Email = TextData.ValidEmail,
            Notes = TextData.MultipleParagraphs,
            Address = ValueObjectData.CompleteAddress,
            PhoneNumbers =
            {
                ValueObjectData.SamplePhoneNumber,
                ValueObjectData.AlternatePhoneNumber,
            },
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            Honorific = string.Empty,
            GivenName = string.Empty,
            FamilyName = string.Empty,
            Title = string.Empty,
            Email = string.Empty,
            Notes = string.Empty,
            Address = null,
        },
        new(new Guid("40000000-0000-0000-0000-000000000003"),
            CustomerData.GetCustomers.ElementAt(1))
        {
            Honorific = "Mx.",
            GivenName = TextData.AnotherWord,
            FamilyName = TextData.EmojiWord,
            Title = TextData.Phrase,
            Email = TextData.ValidEmail,
            Notes = TextData.Paragraph,
            Address = ValueObjectData.IncompleteAddress,
            PhoneNumbers = { ValueObjectData.SamplePhoneNumber },
        },
    };

    private static IEnumerable<Contact>? _contacts;

    public static IEnumerable<Contact> GetContacts
    {
        get
        {
            if (_contacts is not null) return _contacts;
            _contacts = ContactSeedItems;
            return _contacts;
        }
    }

    public static void ClearData() => _contacts = null;
}
