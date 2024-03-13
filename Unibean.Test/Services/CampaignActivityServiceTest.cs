using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class CampaignActivityServiceTest
{
    private readonly ICampaignActivityRepository campaignActivityRepository;

    public CampaignActivityServiceTest()
    {
        campaignActivityRepository = A.Fake<ICampaignActivityRepository>();
    }

    [Fact]
    public void CampaignActivityService_GetAll()
    {
        // Arrange
        List<string> campaignIds = new();
        List<CampaignState> stateIds = new();
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignActivity> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignActivityRepository.GetAll(campaignIds, stateIds, 
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignActivityService(campaignActivityRepository);

        // Act
        var result = service.GetAll(campaignIds, stateIds, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignActivityModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignActivityService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campaignActivityRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new CampaignActivityService(campaignActivityRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampaignActivityModel));
        Assert.Equal(id, result.Id);
    }
}
