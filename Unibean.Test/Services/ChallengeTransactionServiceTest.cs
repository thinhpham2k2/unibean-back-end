using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.ChallengeTransactions;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class ChallengeTransactionServiceTest
{
    private readonly IChallengeTransactionRepository challengeTransRepo;

    public ChallengeTransactionServiceTest()
    {
        challengeTransRepo = A.Fake<IChallengeTransactionRepository>();
    }

    [Fact]
    public void ChallengeTransactionService_Add()
    {
        // Arrange
        string id = "id";
        CreateChallengeTransactionModel creation = A.Fake<CreateChallengeTransactionModel>();
        A.CallTo(() => challengeTransRepo.Add(A<ChallengeTransaction>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new ChallengeTransactionService(challengeTransRepo);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ChallengeTransactionModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void ChallengeTransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> challengeIds = new();
        string search = "";
        List<ChallengeTransaction> pagedResultModel = new()
        {
            new(),
            new(),
            new()
        };
        A.CallTo(() => challengeTransRepo.GetAll(walletIds, challengeIds, search))
            .Returns(pagedResultModel);
        var service = new ChallengeTransactionService(challengeTransRepo);

        // Act
        var result = service.GetAll(walletIds, challengeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<TransactionModel>));
        Assert.Equal(pagedResultModel.Count, result.Count);
    }
}
