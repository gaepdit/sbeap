using Sbeap.Domain.Data;
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
            County = "Bacon",
            Location = ValueObjectData.LessCompleteAddress,
            MailingAddress = ValueObjectData.CompleteAddress,
            Description = TextData.Paragraph,
            SicCode = SicCodeData.GetSicCodes.ElementAt(0),
            Website = TextData.ValidUrl,
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"))
        {
            Name = TextData.Word,
            County = null,
            Location = IncompleteAddress.EmptyAddress,
            MailingAddress = IncompleteAddress.EmptyAddress,
            Description = TextData.Paragraph,
            Website = null,
        },
        new(new Guid("40000000-0000-0000-0000-000000000003"))
        {
            Name = "A Deleted Customer",
            County = null,
            Location = ValueObjectData.AlternateCompleteAddress,
            MailingAddress = IncompleteAddress.EmptyAddress,
            Description = TextData.ShortMultiline,
            SicCode = SicCodeData.GetSicCodes.ElementAt(1),
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
            _customers.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000002");
            return _customers;
        }
    }

    public static void ClearData() => _customers = null;
}
