using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class CampusServiceTest
{
    private readonly ICampusRepository campusRepository;

    private readonly IFireBaseService fireBaseService;

    public CampusServiceTest()
    {
        campusRepository = A.Fake<ICampusRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void CampusService_Add()
    {
        // Arrange
        string id = "id";
        CreateCampusModel creation = A.Fake<CreateCampusModel>();
        A.CallTo(() => campusRepository.Add(A<Campus>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new CampusService(campusRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CampusExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void CampusService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campusRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Students = new List<Student>(),
        });
        A.CallTo(() => campusRepository.Delete(id));
        var service = new CampusService(campusRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void CampusService_GetAll()
    {
        // Arrange
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Campus> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campusRepository.GetAll(universityIds, areaIds, state, 
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampusService(campusRepository, fireBaseService);

        // Act
        var result = service.GetAll(universityIds, areaIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampusModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampusService_GetAllByCampaign()
    {
        // Arrange
        List<string> campaignIds = new();
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Campus> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campusRepository.GetAllByCampaign(campaignIds, universityIds, areaIds,
            state, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampusService(campusRepository, fireBaseService);

        // Act
        var result = service.GetAllByCampaign(campaignIds, universityIds, areaIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampusModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampusService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campusRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new CampusService(campusRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampusExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void CampusService_Update()
    {
        // Arrange
        string id = "id";
        string campusName = "campusName";
        UpdateCampusModel update = A.Fake<UpdateCampusModel>();
        A.CallTo(() => campusRepository.GetById(id));
        A.CallTo(() => campusRepository.Update(A<Campus>.Ignored))
            .Returns(new()
            {
                Id = id,
                CampusName = campusName
            });
        var service = new CampusService(campusRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CampusExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(campusName, result.Result.CampusName);
    }
}
