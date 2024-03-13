using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class CampaignTransactionServiceTest
{
    private readonly ICampaignTransactionRepository campaignTransactionRepository;

    public CampaignTransactionServiceTest()
    {
        campaignTransactionRepository = A.Fake<ICampaignTransactionRepository>();
    }

    [Fact]
    public void CampaignTransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> campaignIds = new();
        List<WalletType> walletTypeIds = new();
        string search = "";
        List<CampaignTransaction> list = new()
        {
            new(),
            new(),
            new()
        };
        A.CallTo(() => campaignTransactionRepository.GetAll(walletIds, campaignIds, walletTypeIds, search))
            .Returns(list);
        var service = new CampaignTransactionService(campaignTransactionRepository);

        // Act
        var result = service.GetAll(walletIds, campaignIds, walletTypeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<TransactionModel>));
        Assert.Equal(list.Count, result.Count);
    }
}
