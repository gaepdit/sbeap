using FluentAssertions.Execution;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class ContactMapping
{
    [Test]
    public void ContactViewMappingWorks()
    {
        var item = new Contact(Guid.NewGuid(), new Customer(Guid.Empty))
        {
            Honorific = TextData.Word,
            GivenName = TextData.AnotherWord,
            FamilyName = TextData.ThirdWord,
            Title = TextData.EmojiWord,
        };

        var result = AppServicesTestsSetup.Mapper!.Map<ContactViewDto>(item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(item.Id);
            result.Honorific.Should().Be(item.Honorific);
            result.GivenName.Should().Be(item.GivenName);
            result.FamilyName.Should().Be(item.FamilyName);
            result.Title.Should().Be(item.Title);
        }
    }

    [Test]
    public void ContactUpdateMappingWorks()
    {
        var customer = new Customer(TextData.TestGuid);
        customer.SetDeleted(null);

        var item = new Contact(Guid.NewGuid(), customer)
        {
            Honorific = TextData.Word,
            GivenName = TextData.AnotherWord,
            FamilyName = TextData.ThirdWord,
            Title = TextData.EmojiWord,
        };
        item.SetDeleted(null);

        var result = AppServicesTestsSetup.Mapper!.Map<ContactUpdateDto>(item);

        using (new AssertionScope())
        {
            result.Honorific.Should().Be(item.Honorific);
            result.GivenName.Should().Be(item.GivenName);
            result.FamilyName.Should().Be(item.FamilyName);
            result.Title.Should().Be(item.Title);
            result.IsDeleted.Should().BeTrue();
            result.CustomerIsDeleted.Should().BeTrue();
            result.CustomerId.Should().Be(TextData.TestGuid);
        }
    }
}
