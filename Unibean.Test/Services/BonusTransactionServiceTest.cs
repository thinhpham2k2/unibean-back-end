using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class BonusTransactionServiceTest
{
    private readonly IBonusTransactionRepository bonusTransactionRepository;

    public BonusTransactionServiceTest()
    {
        bonusTransactionRepository = A.Fake<IBonusTransactionRepository>();
    }

    [Fact]
    public void BonusTransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> bonusIds = new();
        List<WalletType> walletTypeIds = new();
        string search = "";
        List<BonusTransaction> list = new()
        {
            new(),
            new(),
            new()
        };
        A.CallTo(() => bonusTransactionRepository.GetAll(walletIds, bonusIds, walletTypeIds, search))
        .Returns(list);
        var service = new BonusTransactionService(bonusTransactionRepository);

        // Act
        var result = service.GetAll(walletIds, bonusIds, walletTypeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<TransactionModel>));
        Assert.Equal(list.Count, result.Count);
    }
}
