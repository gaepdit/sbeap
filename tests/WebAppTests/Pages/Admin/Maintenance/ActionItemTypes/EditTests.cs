using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Pages.Admin.Maintenance.ActionItemType;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace WebAppTests.Pages.Admin.Maintenance.ActionItemTypes;

public class EditTests
{
    private static readonly ActionItemTypeUpdateDto ItemTest = new(Guid.Empty, TextData.ValidName, true);

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var serviceMock = new Mock<IActionItemTypeService>();
        serviceMock.Setup(l => l.FindActionItemTypeForUpdateAsync(ItemTest.Id, CancellationToken.None)).ReturnsAsync(ItemTest);
        var page = new EditModel(serviceMock.Object, Mock.Of<IValidator<ActionItemTypeUpdateDto>>())
        { TempData = WebAppTestsSetup.PageTempData() };

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
        var serviceMock = new Mock<IActionItemTypeService>();
        var page = new EditModel(serviceMock.Object, Mock.Of<IValidator<ActionItemTypeUpdateDto>>())
        { TempData = WebAppTestsSetup.PageTempData() };

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
        var serviceMock = new Mock<IActionItemTypeService>();
        serviceMock.Setup(l => l.FindActionItemTypeForUpdateAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((ActionItemTypeUpdateDto?)null);
        var page = new EditModel(serviceMock.Object, Mock.Of<IValidator<ActionItemTypeUpdateDto>>())
        { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = new Mock<IActionItemTypeService>();
        var validatorMock = new Mock<IValidator<ActionItemTypeUpdateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<ActionItemTypeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(serviceMock.Object, validatorMock.Object)
        { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
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
        var serviceMock = new Mock<IActionItemTypeService>();
        var validatorMock = new Mock<IValidator<ActionItemTypeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<ActionItemTypeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(serviceMock.Object, validatorMock.Object)
        { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
