using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Test.Services;

public class ActivityServiceTest
{
    private readonly IActivityRepository activityRepository;

    private readonly IVoucherItemRepository voucherItemRepository;

    public ActivityServiceTest()
    {
        activityRepository = A.Fake<IActivityRepository>();
        voucherItemRepository = A.Fake<IVoucherItemRepository>();
    }

    [Fact]
    public void ActivityService_Add()
    {
        // Arrange
        string id = "id";
        Type type = Type.Buy;
        CreateActivityModel creation = A.Fake<CreateActivityModel>();
        A.CallTo(() => voucherItemRepository.GetById(id)).Returns(new());
        A.CallTo(() => activityRepository.Add(A<Activity>.Ignored)).Returns(new()
        {
            VoucherItemId = id,
            Type = type,
            VoucherItem = new()
            {
                CampaignDetail = new()
                {
                    Price = 0,
                    Rate = 0
                }
            }
        });
        var service = new ActivityService(activityRepository, voucherItemRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActivityModel));
        Assert.Equal(id, result.VoucherItemId);
        Assert.Equal(type.ToString(), result.Type);
    }

    [Fact]
    public void ActivityService_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> voucherItemIds = new();
        List<Type> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Activity> pagedResultModel = new()
        {
            Result = new()
            {
                new()
                {
                    Type = Type.Buy,
                    VoucherItem = new()
                    {
                        CampaignDetail = new()
                        {
                            Price = 0,
                            Rate = 0,
                        }
                    }
                },
                new()
                {
                    Type = Type.Use,
                    VoucherItem = new()
                    {
                        CampaignDetail = new()
                        {
                            Price = 0,
                            Rate = 0,
                        }
                    }
                },
                new()
                {
                    Type = Type.Refund,
                    VoucherItem = new()
                    {
                        CampaignDetail = new()
                        {
                            Price = 0,
                            Rate = 0,
                        }
                    }
                }
            }
        };
        A.CallTo(() => activityRepository.GetAll(brandIds, storeIds, studentIds, campaignIds,
            campaignDetailIds, voucherIds, voucherItemIds, typeIds, state, propertySort, isAsc,
            search, page, limit)).Returns(pagedResultModel);
        var service = new ActivityService(activityRepository, voucherItemRepository);

        // Act
        var result = service.GetAll(brandIds, storeIds, studentIds, campaignIds, campaignDetailIds,
            voucherIds, voucherItemIds, typeIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<ActivityModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void ActivityService_GetById()
    {
        // Arrange
        string id = "id";
        Type type = Type.Use;
        A.CallTo(() => activityRepository.GetById(id))
            .Returns(new()
            {
                Id = id,
                Type = type,
                VoucherItem = new()
                {
                    CampaignDetail = new()
                    {
                        Price = 1,
                        Rate = 2,
                    }
                }
            });
        var service = new ActivityService(activityRepository, voucherItemRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActivityExtraModel));
        Assert.Equal(id, result.Id);
        Assert.Equal(type.ToString(), result.Type);
        Assert.Equal(2, result.Amount);
    }

    [Fact]
    public void ActivityService_GetList()
    {
        // Arrange
        string search = "";
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> voucherIds = new();
        List<Activity> activities = new()
        {
            new(), new(), new()
        };
        A.CallTo(() => activityRepository.GetList(storeIds, studentIds, voucherIds, search))
            .Returns(activities);
        var service = new ActivityService(activityRepository, voucherItemRepository);

        // Act
        var result = service.GetList(storeIds, studentIds, voucherIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<StoreTransactionModel>));
        Assert.Equal(activities.Count, result.Count);
    }
}
