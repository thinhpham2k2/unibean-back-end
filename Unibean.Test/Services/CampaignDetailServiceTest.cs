using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class CampaignDetailServiceTest
{
    private readonly ICampaignDetailRepository campaignDetailRepository;

    public CampaignDetailServiceTest()
    {
        campaignDetailRepository = A.Fake<ICampaignDetailRepository>();
    }

    [Fact]
    public void CampaignDetailService_GetAll()
    {
        // Arrange
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignDetail> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignDetailRepository.GetAll(campaignIds, typeIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignDetailService(campaignDetailRepository);

        // Act
        var result = service.GetAll(campaignIds, typeIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignDetailModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignDetailService_GetAllByStore()
    {
        // Arrange
        string storeId = "";
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignDetail> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignDetailRepository.GetAllByStore(storeId, campaignIds, typeIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignDetailService(campaignDetailRepository);

        // Act
        var result = service.GetAllByStore(storeId, campaignIds, typeIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignDetailModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignDetailService_GetAllVoucherItemByCampaignDetail()
    {
        // Arrange
        string id = "";
        List<string> list = new()
        {
            "1",
            "2",
            "3"
        };
        A.CallTo(() => campaignDetailRepository.GetAllVoucherItemByCampaignDetail(id))
            .Returns(list);
        var service = new CampaignDetailService(campaignDetailRepository);

        // Act
        var result = service.GetAllVoucherItemByCampaignDetail(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<string>));
        Assert.Equal(list.Count, list.Count);
    }

    [Fact]
    public void CampaignDetailService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campaignDetailRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new CampaignDetailService(campaignDetailRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampaignDetailExtraModel));
        Assert.Equal(id, result.Id);
    }
}
