using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Areas;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class AreaServiceTest
{

    private readonly IAreaRepository areaRepository;

    private readonly IFireBaseService fireBaseService;

    public AreaServiceTest()
    {
        areaRepository = A.Fake<IAreaRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void AreaService_Add()
    {
        // Arrange
        string id = "id";
        CreateAreaModel creation = A.Fake<CreateAreaModel>();
        A.CallTo(() => areaRepository.Add(A<Area>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new AreaService(areaRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<AreaExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void AreaService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => areaRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Campuses = new List<Campus>(),
            Stores = new List<Store>(),
        });
        A.CallTo(() => areaRepository.Delete(id));
        var service = new AreaService(areaRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void AreaService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Area> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => areaRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new AreaService(areaRepository, fireBaseService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<AreaModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void AreaService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => areaRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new AreaService(areaRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(AreaExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void AreaService_Update()
    {
        // Arrange
        string id = "id";
        string areaName = "areaName";
        UpdateAreaModel update = new();
        A.CallTo(() => areaRepository.GetById(id));
        A.CallTo(() => areaRepository.Update(A<Area>.Ignored))
            .Returns(new()
            {
                Id = id,
                AreaName = areaName
            });
        var service = new AreaService(areaRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<AreaExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(areaName, result.Result.AreaName);
    }
}
