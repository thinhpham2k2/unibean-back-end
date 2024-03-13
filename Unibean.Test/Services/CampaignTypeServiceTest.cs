using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignTypes;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class CampaignTypeServiceTest
{
    private readonly ICampaignTypeRepository campaignTypeRepository;

    private readonly IFireBaseService fireBaseService;

    public CampaignTypeServiceTest()
    {
        campaignTypeRepository = A.Fake<ICampaignTypeRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void CampaignTypeService_Add()
    {
        // Arrange
        string id = "id";
        CreateCampaignTypeModel creation = A.Fake<CreateCampaignTypeModel>();
        A.CallTo(() => campaignTypeRepository.Add(A<CampaignType>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new CampaignTypeService(campaignTypeRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CampaignTypeExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void CampaignTypeService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campaignTypeRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Campaigns = new List<Campaign>(),
        });
        var service = new CampaignTypeService(campaignTypeRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void CampaignTypeService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignType> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignTypeRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new CampaignTypeService(campaignTypeRepository, fireBaseService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignTypeModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignTypeService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campaignTypeRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new CampaignTypeService(campaignTypeRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampaignTypeExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void CampaignTypeService_Update()
    {
        // Arrange
        string id = "id";
        string typeName = "typeName";
        UpdateCampaignTypeModel update = A.Fake<UpdateCampaignTypeModel>();
        A.CallTo(() => campaignTypeRepository.GetById(id));
        A.CallTo(() => campaignTypeRepository.Update(A<CampaignType>.Ignored))
            .Returns(new()
            {
                Id = id,
                TypeName = typeName
            });
        var service = new CampaignTypeService(campaignTypeRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CampaignTypeExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(typeName, result.Result.TypeName);
    }
}
