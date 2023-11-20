using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.SicCodes;
using Sbeap.WebApp.Pages.Customers;

namespace WebAppTests.Pages.Customers;

[TestFixture]
public class AddTests
{
    [Test]
    public async Task OnPostAsync_WhenModelStateIsValid_RedirectsToDetailsPage()
    {
        // Arrange
        var customerDto = new CustomerCreateDto();
        var customerId = Guid.NewGuid();
        var customerService = Substitute.For<ICustomerService>();
        customerService.CreateAsync(customerDto).Returns(customerId);
        var validator = Substitute.For<IValidator<CustomerCreateDto>>();
        validator.ValidateAsync(Arg.Any<CustomerCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new AddModel(customerService, Substitute.For<ISicService>(), validator)
        {
            Item = customerDto,
            TempData = WebAppTestsSetup.PageTempData(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<RedirectToPageResult>();
        var redirectToPageResult = (RedirectToPageResult)result;
        redirectToPageResult.PageName.Should().Be("Details");
        redirectToPageResult.RouteValues!["id"].Should().Be(customerId);
    }

    [Test]
    public async Task OnPostAsync_WhenModelStateIsInvalid_ReturnsPage()
    {
        // Arrange
        var customerDto = new CustomerCreateDto();

        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        var validator = Substitute.For<IValidator<CustomerCreateDto>>();
        validator.ValidateAsync(Arg.Any<CustomerCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var sicService = Substitute.For<ISicService>();
        sicService.GetActiveListItemsAsync().Returns(new List<ListItem<string>>());

        var page = new AddModel(Substitute.For<ICustomerService>(), sicService, validator) { Item = customerDto };
        page.ModelState.AddModelError("", "some error");

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
