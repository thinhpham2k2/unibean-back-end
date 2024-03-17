using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Majors;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class MajorServiceTest
{
    private readonly IMajorRepository majorRepository;

    private readonly IFireBaseService fireBaseService;

    public MajorServiceTest()
    {
        majorRepository = A.Fake<IMajorRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void MajorService_Add()
    {
        // Arrange
        string id = "id";
        CreateMajorModel creation = A.Fake<CreateMajorModel>();
        A.CallTo(() => majorRepository.Add(A<Major>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new MajorService(majorRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<MajorExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void MajorService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => majorRepository.GetById(id)).Returns(new()
        {
            Students = new List<Student>(),
            CampaignMajors = new List<CampaignMajor>()
        });
        A.CallTo(() => majorRepository.Delete(id));
        var service = new MajorService(majorRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void MajorService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Major> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => majorRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new MajorService(majorRepository, fireBaseService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<MajorModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void MajorService_GetAllByCampaign()
    {
        // Arrange
        List<string> campaignIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Major> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => majorRepository.GetAllByCampaign(campaignIds, state, propertySort,
            isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new MajorService(majorRepository, fireBaseService);

        // Act
        var result = service.GetAllByCampaign(campaignIds, state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<MajorModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void MajorService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => majorRepository.GetById(id))
        .Returns(new()
        {
            Id = id
        });
        var service = new MajorService(majorRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(MajorExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void MajorService_Update()
    {
        // Arrange
        string id = "id";
        string majorName = "majorName";
        UpdateMajorModel update = A.Fake<UpdateMajorModel>();
        A.CallTo(() => majorRepository.GetById(id));
        A.CallTo(() => majorRepository.Update(A<Major>.Ignored))
        .Returns(new()
        {
            Id = id,
            MajorName = majorName
        });
        var service = new MajorService(majorRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<MajorExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(majorName, result.Result.MajorName);
    }
}
