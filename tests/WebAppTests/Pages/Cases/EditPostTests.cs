using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.WebApp.Pages.Cases;

namespace WebAppTests.Pages.Cases;

[TestFixture]
public class EditPostTests
{
    private IAgencyService _agencyService = null!;
    private IValidator<CaseworkUpdateDto> _validator = null!;
    private readonly CaseworkUpdateDto _dto = new();

    [SetUp]
    public void SetUp()
    {
        _agencyService = Substitute.For<IAgencyService>();
        _agencyService.GetListItemsAsync().Returns(new List<ListItem>());
        _validator = Substitute.For<IValidator<CaseworkUpdateDto>>();
    }

    [TearDown]
    public void TearDown()
    {
        _agencyService.Dispose();
    }

    [Test]
    public async Task OnPost_GivenEverythingWorks_ShouldReturnRedirectWithMessage()
    {
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(Arg.Any<Guid>()).Returns(_dto);
        var validator = Substitute.For<IValidator<CaseworkUpdateDto>>();
        validator.ValidateAsync(Arg.Any<CaseworkUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var authorizationService = Substitute.For<IAuthorizationService>();
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto,
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Success());
        var guid = Guid.NewGuid();
        var page = new EditModel(caseworkService, _agencyService, validator, authorizationService)
        {
            TempData = WebAppTestsSetup.PageTempData(),
            Id = guid,
            Item = _dto,
        };

        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Case successfully updated.");

        var result = await page.OnPostAsync();

        using var scope = new AssertionScope();
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Details");
        ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(guid);
    }

    [Test]
    public async Task OnPost_WhenNotFound_ShouldReturnBadRequest()
    {
        var guid = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(guid).Returns((CaseworkUpdateDto?)null);
        var page = new EditModel(caseworkService, _agencyService, _validator, Substitute.For<IAuthorizationService>())
            { TempData = WebAppTestsSetup.PageTempData() };


        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_WhenUserDoesNotHaveEditPermission_ShouldReturnBadRequest()
    {
        var guid = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(guid).Returns(_dto);
        var authorizationService = Substitute.For<IAuthorizationService>();
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto,
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Failed());
        var page = new EditModel(caseworkService, _agencyService, _validator, authorizationService)
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ShouldReturnPageWithModelErrors()
    {
        // Arrange
        var agencyService = Substitute.For<IAgencyService>();
        agencyService.GetListItemsAsync().Returns(new List<ListItem>());

        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(Arg.Any<Guid>()).Returns(_dto);

        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        var validator = Substitute.For<IValidator<CaseworkUpdateDto>>();
        validator.ValidateAsync(Arg.Any<CaseworkUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var authorizationService = Substitute.For<IAuthorizationService>();
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto,
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Success());

        var page = new EditModel(caseworkService, agencyService, validator, authorizationService)
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
