using FluentAssertions.Execution;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class CaseworkSearchResultMapping
{
    private static Customer Customer => new(Guid.Empty) { Name = TextData.ValidName };

    [Test]
    public void OpenCaseworkSearchResultMappingWorks()
    {
        var openCasework = new Casework(Guid.Empty, Customer, DateOnly.MinValue)
            { Description = TextData.Paragraph };

        var result = AppServicesTestsSetup.Mapper!.Map<CaseworkSearchResultDto>(openCasework);

        using (new AssertionScope())
        {
            result.Id.Should().Be(openCasework.Id);
            result.CustomerName.Should().Be(Customer.Name);
            result.CaseOpenedDate.Should().Be(openCasework.CaseOpenedDate);
            result.CaseClosedDate.Should().BeNull();
            result.Description.Should().Be(openCasework.Description);
            result.IsClosed.Should().BeFalse();
            result.IsDeleted.Should().BeFalse();
        }
    }

    [Test]
    public void ClosedCaseworkSearchResultMappingWorks()
    {
        var closedCasework = new Casework(Guid.Empty, Customer, DateOnly.MinValue)
            { CaseClosedDate = DateOnly.MaxValue };

        var result = AppServicesTestsSetup.Mapper!.Map<CaseworkSearchResultDto>(closedCasework);

        using (new AssertionScope())
        {
            result.Id.Should().Be(closedCasework.Id);
            result.CaseOpenedDate.Should().Be(closedCasework.CaseOpenedDate);
            result.CaseClosedDate.Should().Be(closedCasework.CaseClosedDate);
            result.IsClosed.Should().BeTrue();
            result.IsDeleted.Should().BeFalse();
        }
    }

    [Test]
    public void DeletedCaseworkSearchResultMappingWorks()
    {
        var deletedCasework = new Casework(Guid.Empty, Customer, DateOnly.MinValue);
        deletedCasework.SetDeleted(Guid.Empty.ToString());

        var result = AppServicesTestsSetup.Mapper!.Map<CaseworkSearchResultDto>(deletedCasework);

        using (new AssertionScope())
        {
            result.Id.Should().Be(deletedCasework.Id);
            result.IsDeleted.Should().BeTrue();
        }
    }
}
