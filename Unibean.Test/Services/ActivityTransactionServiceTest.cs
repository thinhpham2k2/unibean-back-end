using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class ActivityTransactionServiceTest
{
    private readonly IActivityTransactionRepository activityTransactionRepo;

    public ActivityTransactionServiceTest()
    {
        activityTransactionRepo = A.Fake<IActivityTransactionRepository>();
    }

    [Fact]
    public void ActivityTransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> activityIds = new();
        string search = "";
        List<ActivityTransaction> activities = new()
        {
            new(), new(), new()
        };
        A.CallTo(() => activityTransactionRepo.GetAll
            (walletIds, activityIds, search)).Returns(activities);
        var service = new ActivityTransactionService(activityTransactionRepo);

        // Act
        var result = service.GetAll(walletIds, activityIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<TransactionModel>));
        Assert.Equal(activities.Count, result.Count);
    }
}
