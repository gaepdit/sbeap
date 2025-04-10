using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AppServicesTests.Cases;

public class Search
{
    private static CaseworkSearchDto DefaultCaseworkSearch => new();

    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Casework>(CaseworkData.GetCases.ToList());
        var count = CaseworkData.GetCases.Count();
        var paging = new PaginatedRequest(1, 100);

        var caseworkRepoMock = Substitute.For<ICaseworkRepository>();
        caseworkRepoMock.GetPagedListAsync(Arg.Any<Expression<Func<Casework, bool>>>(), Arg.Any<PaginatedRequest>(),
                Arg.Any<string[]?>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        caseworkRepoMock.CountAsync(Arg.Any<Expression<Func<Casework, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var appService = new CaseworkService(AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(),
            caseworkRepoMock, Substitute.For<ICaseworkManager>(), Substitute.For<ICustomerRepository>(),
            Substitute.For<IAgencyRepository>());

        // Act
        var result = await appService.SearchAsync(DefaultCaseworkSearch, paging, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(itemList);
            result.CurrentCount.Should().Be(count);
        }
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Casework>(new List<Casework>());
        const int count = 0;
        var paging = new PaginatedRequest(1, 100);

        var caseworkRepoMock = Substitute.For<ICaseworkRepository>();
        caseworkRepoMock.GetPagedListAsync(Arg.Any<Expression<Func<Casework, bool>>>(), Arg.Any<PaginatedRequest>(),
                Arg.Any<string[]?>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        caseworkRepoMock.CountAsync(Arg.Any<Expression<Func<Casework, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var appService = new CaseworkService(AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(),
            caseworkRepoMock, Substitute.For<ICaseworkManager>(), Substitute.For<ICustomerRepository>(),
            Substitute.For<IAgencyRepository>());

        // Act
        var result = await appService.SearchAsync(DefaultCaseworkSearch, paging, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(itemList);
            result.CurrentCount.Should().Be(count);
        }
    }
}
