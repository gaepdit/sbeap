using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Offices;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Pages.Admin.Maintenance.Offices;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class AddTests
{
    private static readonly OfficeCreateDto ItemTest = new() { Name = TestConstants.ValidName };

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        serviceMock.Setup(l => l.CreateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(Guid.Empty);
        var validatorMock = new Mock<IValidator<OfficeCreateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new AddModel(serviceMock.Object, validatorMock.Object)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully added.");

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.HighlightId.Should().Be(Guid.Empty);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        var validatorMock = new Mock<IValidator<OfficeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new AddModel(serviceMock.Object, validatorMock.Object)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
