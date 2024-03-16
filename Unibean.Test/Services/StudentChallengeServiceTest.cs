using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class StudentChallengeServiceTest
{
    private readonly IStudentChallengeRepository studentChallengeRepository;

    public StudentChallengeServiceTest()
    {
        studentChallengeRepository = A.Fake<IStudentChallengeRepository>();
    }

    [Fact]
    public void StudentChallengeService_Add()
    {
        // Arrange
        string id = "id";
        CreateStudentChallengeModel creation = A.Fake<CreateStudentChallengeModel>();
        A.CallTo(() => studentChallengeRepository.Add(A<StudentChallenge>.Ignored))
            .Returns(new()
            {
                Id = id,
            });
        var service = new StudentChallengeService(studentChallengeRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StudentChallengeModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StudentChallengeService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => studentChallengeRepository.GetById(id))
            .Returns(new()
            {
                Id = id,
            });
        var service = new StudentChallengeService(studentChallengeRepository);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void StudentChallengeService_GetAll()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> challengeIds = new();
        List<ChallengeType> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<StudentChallenge> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => studentChallengeRepository.GetAll(studentIds, challengeIds, typeIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StudentChallengeService(studentChallengeRepository);

        // Act
        var result = service.GetAll(studentIds, challengeIds, typeIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StudentChallengeModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StudentChallengeService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => studentChallengeRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new StudentChallengeService(studentChallengeRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StudentChallengeModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StudentChallengeService_Update()
    {
        // Arrange
        decimal amount = 0;
        List<StudentChallenge> studentChallenges = new();
        A.CallTo(() => studentChallengeRepository.Update(A<StudentChallenge>.Ignored));
        var service = new StudentChallengeService(studentChallengeRepository);

        // Act & Assert
        service.Update(studentChallenges, amount);
    }
}
