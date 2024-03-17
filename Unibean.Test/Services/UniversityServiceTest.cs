using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Universities;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class UniversityServiceTest
{
    private readonly IUniversityRepository universityRepository;

    private readonly IFireBaseService fireBaseService;

    public UniversityServiceTest()
    {
        universityRepository = A.Fake<IUniversityRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void UniversityService_Add()
    {
        // Arrange
        string id = "id";
        CreateUniversityModel creation = A.Fake<CreateUniversityModel>();
        A.CallTo(() => universityRepository.Add(A<University>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new UniversityService(universityRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<UniversityExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void UniversityService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => universityRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Campuses = new List<Campus>(),
        });
        A.CallTo(() => universityRepository.Delete(id));
        var service = new UniversityService(universityRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void UniversityService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<University> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => universityRepository.GetAll(state, propertySort, isAsc,
            search, page, limit)).Returns(pagedResultModel);
        var service = new UniversityService(universityRepository, fireBaseService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<UniversityModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void UniversityService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => universityRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new UniversityService(universityRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(UniversityExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void UniversityService_Update()
    {
        // Arrange
        string id = "id";
        string campusName = "campusName";
        UpdateUniversityModel update = A.Fake<UpdateUniversityModel>();
        A.CallTo(() => universityRepository.GetById(id));
        A.CallTo(() => universityRepository.Update(A<University>.Ignored))
            .Returns(new()
            {
                Id = id,
                UniversityName = campusName
            });
        var service = new UniversityService(universityRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<UniversityExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(campusName, result.Result.UniversityName);
    }
}
