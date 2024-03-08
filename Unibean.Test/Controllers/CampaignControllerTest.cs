using FakeItEasy;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class CampaignControllerTest
{
    private readonly ICampaignService campaignService;

    private readonly IJwtService jwtService;

    public CampaignControllerTest()
    {
        campaignService = A.Fake<ICampaignService>();
        jwtService = A.Fake<IJwtService>();
    }
}
