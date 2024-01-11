using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.WebApp.Pages.Cases;

namespace WebAppTests.Pages.Cases;

[TestFixture]
public class DetailsTests
{
    private IActionItemService _actionItemService = null!;
    private IActionItemTypeService _actionItemTypeService = null!;
    private IAuthorizationService _authorizationService = null!;

    [SetUp]
    public void SetUp()
    {
        _actionItemService = Substitute.For<IActionItemService>();
        _actionItemTypeService = Substitute.For<IActionItemTypeService>();
        _authorizationService = Substitute.For<IAuthorizationService>();
    }

    [TearDown]
    public void TearDown()
    {
        _actionItemService.Dispose();
        _actionItemTypeService.Dispose();
    }

    private DetailsModel BuildPageModel(ICaseworkService caseworkService) =>
        new(caseworkService, _actionItemService, _actionItemTypeService, _authorizationService);

    [Test]
    public async Task OnGetAsync_NullId_ReturnsRedirectToPageResult()
    {
        var page = BuildPageModel(Substitute.For<ICaseworkService>());
        var result = await page.OnGetAsync(null);
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnGetAsync_CaseNotFound_ReturnsNotFoundResult()
    {
        var id = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindAsync(id).Returns((CaseworkViewDto?)null);
        var page = BuildPageModel(caseworkService);

        var result = await page.OnGetAsync(id);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        var page = BuildPageModel(Substitute.For<ICaseworkService>());
        var result = await page.OnPostAsync(null);
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnPostAsync_CaseNotFound_ReturnsBadRequestResult()
    {
        var id = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        caseworkService.FindAsync(id).Returns((CaseworkViewDto?)null);
        var page = BuildPageModel(caseworkService);
        page.NewActionItem = new ActionItemCreateDto(id);

        var result = await page.OnPostAsync(id);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        var id = Guid.NewGuid();
        var caseworkService = Substitute.For<ICaseworkService>();
        var page = BuildPageModel(caseworkService);
        page.NewActionItem = new ActionItemCreateDto(Guid.NewGuid());

        var result = await page.OnPostAsync(id);

        result.Should().BeOfType<BadRequestResult>();
    }
}
