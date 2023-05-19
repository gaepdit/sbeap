using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AppServicesTests.Customers;

public class Search
{
    private static CustomerSearchDto DefaultCustomerSearchDto => new(CustomerSortBy.Name, null, null, null, null);

    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Customer>(CustomerData.GetCustomers.ToList());
        var count = CustomerData.GetCustomers.Count();
        var paging = new PaginatedRequest(1, 100);

        var customerRepoMock = new Mock<ICustomerRepository>();
        customerRepoMock.Setup(l => l.GetPagedListAsync(It.IsAny<Expression<Func<Customer, bool>>>(),
                It.IsAny<PaginatedRequest>(), CancellationToken.None))
            .ReturnsAsync(itemList);
        customerRepoMock.Setup(l => l.CountAsync(It.IsAny<Expression<Func<Customer, bool>>>(), CancellationToken.None))
            .ReturnsAsync(count);

        var appService = new CustomerService(AppServicesTestsSetup.Mapper!, Mock.Of<IUserService>(),
            Mock.Of<IStaffService>(), customerRepoMock.Object, Mock.Of<ICustomerManager>(),
            Mock.Of<IContactRepository>());

        // Act
        var result = await appService.SearchAsync(DefaultCustomerSearchDto, paging, CancellationToken.None);

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
        var itemList = new ReadOnlyCollection<Customer>(new List<Customer>());
        const int count = 0;
        var paging = new PaginatedRequest(1, 100);

        var customerRepoMock = new Mock<ICustomerRepository>();
        customerRepoMock.Setup(l => l.GetPagedListAsync(It.IsAny<Expression<Func<Customer, bool>>>(),
                It.IsAny<PaginatedRequest>(), CancellationToken.None))
            .ReturnsAsync(itemList);
        customerRepoMock.Setup(l => l.CountAsync(It.IsAny<Expression<Func<Customer, bool>>>(), CancellationToken.None))
            .ReturnsAsync(count);

        var appService = new CustomerService(AppServicesTestsSetup.Mapper!, Mock.Of<IUserService>(),
            Mock.Of<IStaffService>(), customerRepoMock.Object, Mock.Of<ICustomerManager>(),
            Mock.Of<IContactRepository>());

        // Act
        var result = await appService.SearchAsync(DefaultCustomerSearchDto, paging, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(itemList);
            result.CurrentCount.Should().Be(count);
        }
    }
}
