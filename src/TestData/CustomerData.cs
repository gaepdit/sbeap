using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData.Constants;

namespace Sbeap.TestData;

internal static class CustomerData
{
    private static IEnumerable<Customer> CustomerSeedItems => new List<Customer>
    {
        new(new Guid("40000000-0000-0000-0000-000000000001"))
        {
            Name = TextData.Word,
            County = TextData.AnotherWord,
            Location = ValueObjectData.LessCompleteAddress,
            MailingAddress = ValueObjectData.CompleteAddress,
            Description = TextData.Phrase,
            WebSite = TextData.ValidUrl,
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"))
        {
            Name = string.Empty,
            County = null,
            Location = null,
            MailingAddress = null,
            Description = string.Empty,
            WebSite = null,
        },
    };

    private static IEnumerable<Customer>? _customers;

    public static IEnumerable<Customer> GetCustomers
    {
        get
        {
            if (_customers is not null) return _customers;
            _customers = CustomerSeedItems;
            return _customers;
        }
    }

    public static void ClearData() => _customers = null;
}
