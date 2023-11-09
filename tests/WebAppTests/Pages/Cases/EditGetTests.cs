using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.WebApp.Pages.Cases;

namespace WebAppTests.Pages.Cases;

[TestFixture]
public class EditGetTests
{
    private IAgencyService _agencyService = null!;
    private IValidator<CaseworkUpdateDto> _validator = null!;
    private readonly CaseworkUpdateDto _dto = new();

    [SetUp]
    public void Setup()
    {
        _agencyService = Substitute.For<IAgencyService>();
        _validator = Substitute.For<IValidator<CaseworkUpdateDto>>();
    }

    [TearDown]
    public void Teardown()
    {
        _agencyService.Dispose();
    }

    private EditModel CreatePageModel(ICaseworkService caseworkService, IAuthorizationService authorizationService) =>
        new(caseworkService, _agencyService, _validator, authorizationService)
            { TempData = WebAppTestsSetup.PageTempData() };

    [Test]
    public async Task OnGet_WhenIdIsValid_ShouldReturnPage()
    {
        var guid = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(guid).Returns(_dto);
        var authorizationService = Substitute.For<IAuthorizationService>();
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto, Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Success());
        var page = CreatePageModel(caseworkService, authorizationService);

        var result = await page.OnGetAsync(guid);

        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.Id.Should().Be(guid);
        page.Item.Should().Be(_dto);
    }


    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var caseworkService = Substitute.For<ICaseworkService>();
        var page = CreatePageModel(caseworkService, Substitute.For<IAuthorizationService>());
        var result = await page.OnGetAsync(null);

        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var guid = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(guid, Arg.Any<CancellationToken>())
            .Returns((CaseworkUpdateDto?)null);
        var page = CreatePageModel(caseworkService, Substitute.For<IAuthorizationService>());

        var result = await page.OnGetAsync(guid);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnGet_WhenUserHasNoPermissions_ShouldReturnNotFound()
    {
        var guid = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(guid).Returns(_dto);
        var authorizationService = Substitute.For<IAuthorizationService>();
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto,
                Arg.Any<IAuthorizationRequirement[]>())
            .Returns(AuthorizationResult.Failed());
        var page = CreatePageModel(caseworkService, authorizationService);

        var result = await page.OnGetAsync(guid);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnGet_WhenUserHasManageDeletionsPermissionButNotEdit_ShouldRedirectWithMessage()
    {
        var guid = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindForUpdateAsync(guid).Returns(_dto);
        var authorizationService = Substitute.For<IAuthorizationService>();
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto,
                Arg.Is<IAuthorizationRequirement[]>(x => x.Contains(CaseworkOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Success());
        authorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), _dto,
                Arg.Is<IAuthorizationRequirement[]>(x => !x.Contains(CaseworkOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Failed());
        var page = CreatePageModel(caseworkService, authorizationService);
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted case.");

        var result = await page.OnGetAsync(guid);

        using var scope = new AssertionScope();
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Details");
        ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(guid);
    }
}
