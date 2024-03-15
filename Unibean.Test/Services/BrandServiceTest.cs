using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class BrandServiceTest
{
    private readonly IBrandRepository brandRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    private readonly ICampaignService campaignService;

    private readonly IStoreService storeService;

    private readonly IVoucherService voucherService;

    private readonly IEmailService emailService;

    private readonly ITransactionService transactionService;

    public BrandServiceTest()
    {
        brandRepository = A.Fake<IBrandRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        accountRepository = A.Fake<IAccountRepository>();
        campaignService = A.Fake<ICampaignService>();
        storeService = A.Fake<IStoreService>();
        voucherService = A.Fake<IVoucherService>();
        emailService = A.Fake<IEmailService>();
        transactionService = A.Fake<ITransactionService>();
    }

    [Fact]
    public void BrandService_Add()
    {
        // Arrange
        string id = "id";
        CreateBrandModel creation = A.Fake<CreateBrandModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored)).Returns(new());
        A.CallTo(() => brandRepository.Add(A<Brand>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<BrandExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void BrandService_AddGoogle()
    {
        // Arrange
        string id = "id";
        CreateBrandGoogleModel creation = A.Fake<CreateBrandGoogleModel>();
        A.CallTo(() => brandRepository.Add(A<Brand>.Ignored))
            .Returns(new()
            {
                Id = id
            });
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.AddGoogle(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BrandModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void BrandService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => brandRepository.GetById(id)).Returns(new()
        {
            Account = new()
            {
                Id = id,
            },
            Campaigns = new List<Campaign>(),
        });
        A.CallTo(() => brandRepository.Delete(id));
        A.CallTo(() => accountRepository.Delete(id));
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void BrandService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        JwtRequestModel request = new()
        {
            Role = "Admin"
        };
        PagedResultModel<Brand> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => brandRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit, request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<BrandModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void BrandService_GetById()
    {
        // Arrange
        string id = "id";
        JwtRequestModel request = new()
        {
            Role = "Admin"
        };
        A.CallTo(() => brandRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.GetById(id, request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BrandExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void BrandService_GetCampaignListByBrandId()
    {
        // Arrange
        string id = "";
        List<string> typeIds = new();
        List<string> storeIds = new();
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<CampaignState> stateIds = new();
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => brandRepository.GetById(id)).Returns(new());
        A.CallTo(() => campaignService.GetAll(new() { id }, typeIds, storeIds, majorIds,
                campusIds, stateIds, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.GetCampaignListByBrandId
            (id, typeIds, storeIds, majorIds, campusIds, stateIds,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void BrandService_GetHistoryTransactionListByBrandId()
    {
        // Arrange
        string id = "";
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<TransactionModel> list = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => brandRepository.GetById(id)).Returns(new()
        {
            Wallets = new List<Wallet>
            {
                new()
                {
                    Id = id,
                }
            }
        });
        A.CallTo(() => transactionService.GetAll(new() { id }, new(), state, propertySort, isAsc, search, page, limit, Role.Brand))
            .Returns(list);
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.GetHistoryTransactionListByBrandId
            (id, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<TransactionModel>));
        Assert.Equal(list.Result.Count, result.Result.Count);
    }

    [Fact]
    public void BrandService_GetStoreListByBrandId()
    {
        // Arrange
        string id = "";
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        JwtRequestModel request = new()
        {
            Role = "Admin"
        };
        PagedResultModel<StoreModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => brandRepository.GetById(id)).Returns(new());
        A.CallTo(() => storeService.GetAll(new() { id }, areaIds, state,
                propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.GetStoreListByBrandId
            (id, areaIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StoreModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void BrandService_GetVoucherListByBrandId()
    {
        // Arrange
        string id = "";
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        JwtRequestModel request = new()
        {
            Role = "Admin"
        };
        PagedResultModel<VoucherModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => brandRepository.GetById(id)).Returns(new());
        A.CallTo(() => voucherService.GetAll(new() { id }, typeIds, state,
                propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.GetVoucherListByBrandId
            (id, typeIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<VoucherModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void BrandService_Update()
    {
        // Arrange
        string id = "id";
        string brandName = "brandName";
        UpdateBrandModel update = A.Fake<UpdateBrandModel>();
        A.CallTo(() => brandRepository.GetById(id));
        A.CallTo(() => brandRepository.Update(A<Brand>.Ignored))
            .Returns(new()
            {
                Id = id,
                BrandName = brandName
            });
        var service = new BrandService(brandRepository, fireBaseService, accountRepository,
            campaignService, storeService, voucherService, emailService, transactionService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<BrandExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(brandName, result.Result.BrandName);
    }
}
