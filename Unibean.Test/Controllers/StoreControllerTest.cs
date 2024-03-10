using FakeItEasy;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class StoreControllerTest
{
    private readonly IStoreService storeService;

    private readonly IBonusService bonusService;

    private readonly IChartService chartService;

    public StoreControllerTest()
    {
        storeService = A.Fake<IStoreService>();
        bonusService = A.Fake<IBonusService>();
        chartService = A.Fake<IChartService>();
    }
}
