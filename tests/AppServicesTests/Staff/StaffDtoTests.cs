using FluentAssertions.Execution;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.TestData.Constants;

namespace AppServicesTests.Staff;

public class StaffDtoTests
{
    [Test]
    public void DisplayName_ExpectedBehavior()
    {
        var staffSearchDto = new StaffSearchDto { Name = " abc ", Email = " def " };

        staffSearchDto.TrimAll();

        using (new AssertionScope())
        {
            staffSearchDto.Name.Should().Be("abc");
            staffSearchDto.Email.Should().Be("def");
        }
    }

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = new StaffViewDto { GivenName = givenName, FamilyName = familyName };
        staffViewDto.DisplayName.Should().Be(expected);
    }

    [TestCase("abc", "def", "def, abc")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void SortableFullName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = new StaffViewDto { GivenName = givenName, FamilyName = familyName };
        staffViewDto.SortableFullName.Should().Be(expected);
    }

    [Test]
    public void AsUpdateDto_ExpectedBehavior()
    {
        var staffViewDto = new StaffViewDto
        {
            Phone = TestConstants.ValidPhoneNumber,
            Office = new OfficeViewDto { Id = Guid.NewGuid() },
        };

        var result = staffViewDto.AsUpdateDto();

        using (new AssertionScope())
        {
            result.Active.Should().BeTrue();
            result.Phone.Should().Be(staffViewDto.Phone);
            result.OfficeId.Should().Be(staffViewDto.Office.Id);
        }
    }
}
