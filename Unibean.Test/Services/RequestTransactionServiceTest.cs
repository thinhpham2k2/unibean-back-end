using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class RequestTransactionServiceTest
{
    private readonly IRequestTransactionRepository requestTransactionRepository;

    public RequestTransactionServiceTest()
    {
        requestTransactionRepository = A.Fake<IRequestTransactionRepository>();
    }

    [Fact]
    public void RequestTransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> requestIds = new();
        List<WalletType> walletTypeIds = new();
        string search = "";
        List<RequestTransaction> requests = new()
        {
            new(), new(), new()
        };
        A.CallTo(() => requestTransactionRepository.GetAll
            (walletIds, requestIds, walletTypeIds, search)).Returns(requests);
        var service = new RequestTransactionService(requestTransactionRepository);

        // Act
        var result = service.GetAll(walletIds, requestIds, walletTypeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<TransactionModel>));
        Assert.Equal(requests.Count, result.Count);
    }
}
