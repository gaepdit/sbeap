using Sbeap.AppServices.Offices;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices;

public class GetActiveStaff
{
    [Test]
    public async Task WhenOfficeExists_ReturnsViewDtoList()
    {
        var user = new ApplicationUser
        {
            GivenName = TextData.ValidName,
            FamilyName = TextData.NewValidName,
            Email = TextData.ValidEmail,
        };

        var itemList = new List<ApplicationUser> { user };
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.GetActiveStaffMembersListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();

        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.GetActiveStaffAsync(Guid.Empty);

        result.Should().ContainSingle(e =>
            string.Equals(e.GivenName, user.GivenName, StringComparison.Ordinal) &&
            string.Equals(e.FamilyName, user.FamilyName, StringComparison.Ordinal) &&
            string.Equals(e.Email, user.Email, StringComparison.Ordinal));
    }
}
