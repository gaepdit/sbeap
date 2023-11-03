using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.WebApp.Pages.Cases;

namespace WebAppTests.Pages.Cases
{
    [TestFixture]
    public class DetailsTests
    {
        private DetailsModel _page = null!;
        private ICaseworkService _caseworkService = null!;
        private IActionItemService _actionItemService = null!;
        private IActionItemTypeService _actionItemTypeService = null!;
        private IAuthorizationService _authorizationService = null!;

        [SetUp]
        public void SetUp()
        {
            _caseworkService = Substitute.For<ICaseworkService>();
            _actionItemService = Substitute.For<IActionItemService>();
            _actionItemTypeService = Substitute.For<IActionItemTypeService>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _page = new DetailsModel(_caseworkService, _actionItemService, _actionItemTypeService,
                _authorizationService);
        }

        [TearDown]
        public void TearDown()
        {
            _caseworkService.Dispose();
            _actionItemService.Dispose();
            _actionItemTypeService.Dispose();
        }

        [Test]
        public async Task OnGetAsync_NullId_ReturnsRedirectToPageResult()
        {
            var result = await _page.OnGetAsync(null);
            result.Should().BeOfType<RedirectToPageResult>();
        }

        [Test]
        public async Task OnGetAsync_CaseNotFound_ReturnsNotFoundResult()
        {
            var id = Guid.NewGuid();
            _caseworkService.FindAsync(id).Returns((CaseworkViewDto?)null);

            var result = await _page.OnGetAsync(id);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
        {
            var result = await _page.OnPostAsync(null);
            result.Should().BeOfType<RedirectToPageResult>();
        }

        [Test]
        public async Task OnPostAsync_CaseNotFound_ReturnsNotFoundResult()
        {
            var id = Guid.NewGuid();
            _caseworkService.FindAsync(id).Returns((CaseworkViewDto?)null);

            var result = await _page.OnPostAsync(id);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
