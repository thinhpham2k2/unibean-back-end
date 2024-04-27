using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class StoreServiceTest
{
    private readonly IStoreRepository storeRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    private readonly ICampaignDetailService campaignDetailService;

    private readonly IActivityService activityService;

    private readonly IBonusService bonusService;

    private readonly IVoucherItemRepository voucherItemRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IVoucherItemService voucherItemService;

    public StoreServiceTest()
    {
        storeRepository = A.Fake<IStoreRepository>();
        fireBaseService = A.Fake<IFireBaseService>(); ;
        accountRepository = A.Fake<IAccountRepository>(); ;
        campaignDetailService = A.Fake<ICampaignDetailService>(); ;
        activityService = A.Fake<IActivityService>(); ;
        bonusService = A.Fake<IBonusService>(); ;
        voucherItemRepository = A.Fake<IVoucherItemRepository>(); ;
        studentRepository = A.Fake<IStudentRepository>(); ;
        voucherItemService = A.Fake<IVoucherItemService>(); ;
    }

    [Fact]
    public void StoreService_Add()
    {
        // Arrange
        string id = "id";
        CreateStoreModel creation = A.Fake<CreateStoreModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored)).Returns(new());
        A.CallTo(() => storeRepository.Add(A<Store>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StoreExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void StoreService_AddActivity()
    {
        // Arrange
        string id = "id";
        string code = "code";
        string name = "name";
        string brandId = "brandId";
        CreateUseActivityModel creation = A.Fake<CreateUseActivityModel>();
        A.CallTo(() => voucherItemRepository.GetByVoucherCode(code, brandId)).Returns(new()
        {
            IsLocked = true,
            VoucherCode = code,
            IsBought = true,
            IsUsed = false,
            CampaignDetail = new()
            {
                Campaign = new()
                {
                    StartOn = DateOnly.FromDateTime(DateTime.Now),
                    EndOn = DateOnly.FromDateTime(DateTime.Now),
                    CampaignActivities = new List<CampaignActivity>()
                    {
                        new()
                        {
                            State = CampaignState.Active,
                        }
                    },
                    CampaignStores = new List<CampaignStore>()
                    {
                        new()
                        {
                            StoreId = id,
                        }
                    }
                },
                Price = 2,
                Rate = 2,
            },
            Activities = new List<Activity>()
            {
                new()
                {
                    StudentId = id,
                }
            },
            Voucher = new()
            {
                VoucherName = name
            }
        });
        A.CallTo(() => storeRepository.GetById(id)).Returns(new()
        {
            BrandId = brandId,
        });
        A.CallTo(() => studentRepository.GetById(id)).Returns(new()
        {
            Id = id,
            State = StudentState.Active
        });
        A.CallTo(() => activityService.Add(A<CreateActivityModel>.Ignored)).Returns(new());
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act & Assert
        Assert.True(service.AddActivity(id, code, creation));
    }

    [Fact]
    public void StoreService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => storeRepository.GetById(id)).Returns(new()
        {
            Account = new()
            {
                Id = id,
            },
            CampaignStores = new List<CampaignStore>()
            {
                new()
                {
                    Campaign = new()
                    {
                        CampaignActivities = new List<CampaignActivity>()
                        {
                            new()
                            {
                                State = CampaignState.Closed
                            }
                        }
                    }
                }
            }
        });
        A.CallTo(() => storeRepository.Delete(id));
        A.CallTo(() => accountRepository.Delete(id));
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void StoreService_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Store> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => storeRepository.GetAll(brandIds, areaIds, state, propertySort,
            isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetAll(brandIds, areaIds, state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StoreModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StoreService_GetAllByCampaign()
    {
        // Arrange
        List<string> campaignIds = new();
        List<string> brandIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Store> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => storeRepository.GetAllByCampaign(campaignIds, brandIds, areaIds,
            state, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetAllByCampaign(campaignIds, brandIds, areaIds, state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StoreModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StoreService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => storeRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StoreExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StoreService_GetHistoryTransactionListByStoreId()
    {
        // Arrange
        string id = "id";
        List<StoreTransactionType> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        List<StoreTransactionModel> list = new()
        {
            new(),
            new(),
            new()
        };
        PagedResultModel<StoreTransactionModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new(),
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => activityService.GetList(new() { id }, new(), new(), search))
            .Returns(list);
        A.CallTo(() => bonusService.GetList(new(), new() { id }, new(), search))
            .Returns(list);
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetHistoryTransactionListByStoreId(id, typeIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StoreTransactionModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StoreService_GetCampaignDetailByStoreId()
    {
        // Arrange
        string id = "id";
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignDetailModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new(),
            }
        };
        A.CallTo(() => storeRepository.GetById(id));
        A.CallTo(() => campaignDetailService.GetAllByStore(id, campaignIds, typeIds,
            state, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetCampaignDetailByStoreId(id, campaignIds, typeIds,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignDetailModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StoreService_Update()
    {
        // Arrange
        string id = "id";
        string storeName = "storeName";
        UpdateStoreModel update = A.Fake<UpdateStoreModel>();
        A.CallTo(() => storeRepository.GetById(id));
        A.CallTo(() => storeRepository.Update(A<Store>.Ignored))
        .Returns(new()
        {
            Id = id,
            StoreName = storeName
        });
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StoreExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(storeName, result.Result.StoreName);
    }

    [Fact]
    public void StoreService_GetCampaignDetailById()
    {
        // Arrange
        string id = "id";
        string detailId = "detailId";
        A.CallTo(() => storeRepository.GetById(id));
        A.CallTo(() => campaignDetailService.GetById(detailId))
            .Returns(new()
            {
                Id = detailId
            });
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetCampaignDetailById(id, detailId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampaignDetailExtraModel));
        Assert.Equal(detailId, result.Id);
    }

    [Fact]
    public void StoreService_GetVoucherItemByCode()
    {
        // Arrange
        string id = "id";
        string code = "code";
        string brandId = "brandId";
        A.CallTo(() => voucherItemRepository.GetByVoucherCode(code, brandId)).Returns(new()
        {
            IsBought = true,
            IsUsed = false,
            ValidOn = DateOnly.FromDateTime(DateTime.Now),
            ExpireOn = DateOnly.FromDateTime(DateTime.Now),
            Activities = new List<Activity>()
            {
                new()
            },
            CampaignDetail = new()
            {
                Campaign = new()
                {
                    CampaignStores = new List<CampaignStore>()
                    {
                        new()
                        {
                            StoreId = id,
                        }
                    },
                    CampaignActivities = new List<CampaignActivity>()
                    {
                        new()
                        {
                            Id = id,
                        }
                    }
                }
            }
        });
        A.CallTo(() => storeRepository.GetById(id)).Returns(new()
        {
            BrandId = brandId,
        });
        A.CallTo(() => voucherItemService.EntityToExtra(A<VoucherItem>.Ignored))
            .Returns(new()
            {
                VoucherCode = code,
            });
        var service = new StoreService
            (storeRepository, fireBaseService, accountRepository,
            campaignDetailService, activityService, bonusService,
            voucherItemRepository, studentRepository, voucherItemService);

        // Act
        var result = service.GetVoucherItemByCode(id, code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherItemExtraModel));
        Assert.Equal(code, result.VoucherCode);
    }
}
