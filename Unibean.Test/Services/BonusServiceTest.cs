using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Bonuses;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class BonusServiceTest
{
    private readonly IBonusRepository bonusRepository;

    private readonly IStoreRepository storeRepository;

    private readonly IFireBaseService fireBaseService;

    public BonusServiceTest()
    {
        bonusRepository = A.Fake<IBonusRepository>();
        storeRepository = A.Fake<IStoreRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void BonusService_Add()
    {
        // Arrange
        string id = "id";
        CreateBonusModel creation = new()
        {
            Amount = 1
        };
        A.CallTo(() => storeRepository.GetById(id)).Returns(new()
        {
            Brand = new()
            {
                Wallets = new List<Wallet>() {
                    new()
                    {
                        Type = WalletType.Green,
                        Balance = 10
                    }
                }
            }
        });
        A.CallTo(() => bonusRepository.Add(A<Bonus>.Ignored)).Returns(new()
        {
            StoreId = id,
        });
        var service = new BonusService(bonusRepository, storeRepository, fireBaseService);

        // Act
        var result = service.Add(id, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BonusExtraModel));
        Assert.Equal(id, result.StoreId);
    }

    [Fact]
    public void BonusService_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Bonus> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => bonusRepository.GetAll(brandIds, storeIds, studentIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new BonusService(bonusRepository, storeRepository, fireBaseService);

        // Act
        var result = service.GetAll(brandIds, storeIds, studentIds,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<BonusModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void BonusService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => bonusRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new BonusService(bonusRepository, storeRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BonusExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void BonusService_GetList()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        string search = "";
        List<Bonus> list = new()
        {
            new(),
            new(),
            new()
        };
        A.CallTo(() => bonusRepository.GetList(brandIds, storeIds, studentIds, search))
            .Returns(list);
        var service = new BonusService(bonusRepository, storeRepository, fireBaseService);

        // Act
        var result = service.GetList(brandIds, storeIds, studentIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<StoreTransactionModel>));
        Assert.Equal(list.Count, result.Count);
    }
}
