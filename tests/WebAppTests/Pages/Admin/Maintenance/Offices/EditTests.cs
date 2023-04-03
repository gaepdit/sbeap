using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Offices;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Pages.Admin.Maintenance.Offices;
using Sbeap.WebApp.Platform.RazorHelpers;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class EditTests
{
    private static readonly OfficeUpdateDto ItemTest = new() { Id = Guid.Empty, Name = TestConstants.ValidName };

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        serviceMock.Setup(l => l.FindForUpdateAsync(ItemTest.Id, CancellationToken.None)).ReturnsAsync(ItemTest);
        var page = new EditModel(serviceMock.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.PageTempData() };

        await page.OnGetAsync(ItemTest.Id);

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(ItemTest);
            page.OriginalName.Should().Be(ItemTest.Name);
            page.HighlightId.Should().Be(Guid.Empty);
        }
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        var page = new EditModel(serviceMock.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        serviceMock.Setup(l => l.FindForUpdateAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((OfficeUpdateDto?)null);
        var page = new EditModel(serviceMock.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        var validatorMock = new Mock<IValidator<OfficeUpdateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<OfficeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(serviceMock.Object, validatorMock.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully updated.");

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.HighlightId.Should().Be(ItemTest.Id);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        var validatorMock = new Mock<IValidator<OfficeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<OfficeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(serviceMock.Object, validatorMock.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
