using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
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
