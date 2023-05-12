using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.ValueObjects;
using Sbeap.TestData.Constants;

namespace Sbeap.TestData;

internal static class CustomerData
{
    private static IEnumerable<Customer> CustomerSeedItems => new List<Customer>
    {
        new(new Guid("40000000-0000-0000-0000-000000000001"))
        {
            Name = TextData.Phrase,
            County = TextData.AnotherWord,
            Location = ValueObjectData.LessCompleteAddress,
            MailingAddress = ValueObjectData.CompleteAddress,
            Description = TextData.Paragraph,
            Website = TextData.ValidUrl,
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"))
        {
            Name = TextData.EmojiWord,
            County = null,
            Location = IncompleteAddress.EmptyAddress,
            MailingAddress = IncompleteAddress.EmptyAddress,
            Description = TextData.Paragraph,
            Website = null,
        },
        new(new Guid("40000000-0000-0000-0000-000000000003"))
        {
            Name = "Deleted Customer",
            County = null,
            Location = IncompleteAddress.EmptyAddress,
            MailingAddress = IncompleteAddress.EmptyAddress,
            Description = TextData.ShortMultiline,
            Website = null,
            DeleteComments = TextData.ShortMultiline,
        },
    };

    private static IEnumerable<Customer>? _customers;

    public static IEnumerable<Customer> GetCustomers
    {
        get
        {
            if (_customers is not null) return _customers;
            _customers = CustomerSeedItems.ToList();
            _customers.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000001");

            foreach (var customer in _customers)
            {
                customer.Contacts = ContactData.GetContacts
                    .Where(e => e.Customer.Id == customer.Id).ToList();
                customer.Cases = CaseworkData.GetCases
                    .Where(e => e.Customer.Id == customer.Id).ToList();
            }

            return _customers;
        }
    }

    public static void ClearData() => _customers = null;
}
