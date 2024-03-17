using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Wallets;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class WalletServiceTest
{
    private readonly IWalletRepository walletRepository;

    public WalletServiceTest()
    {
        walletRepository = A.Fake<IWalletRepository>();
    }

    [Fact]
    public void WalletService_Add()
    {
        // Arrange
        string id = "id";
        CreateWalletModel creation = A.Fake<CreateWalletModel>();
        A.CallTo(() => walletRepository.Add(A<Wallet>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new WalletService(walletRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(WalletModel));
        Assert.Equal(id, result.Id);
    }
}
