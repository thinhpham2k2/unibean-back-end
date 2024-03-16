using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class OrderTransactionServiceTest
{
    private readonly IOrderTransactionRepository orderTransactionRepo;

    public OrderTransactionServiceTest()
    {
        orderTransactionRepo = A.Fake<IOrderTransactionRepository>();
    }

    [Fact]
    public void OrderTransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> orderIds = new();
        string search = "";
        List<OrderTransaction> activities = new()
        {
            new(), new(), new()
        };
        A.CallTo(() => orderTransactionRepo.GetAll
            (walletIds, orderIds, search)).Returns(activities);
        var service = new OrderTransactionService(orderTransactionRepo);

        // Act
        var result = service.GetAll(walletIds, orderIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<TransactionModel>));
        Assert.Equal(activities.Count, result.Count);
    }
}
