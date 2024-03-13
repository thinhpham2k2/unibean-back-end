using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Challenges;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class ChallengeServiceTest
{
    private readonly IChallengeRepository challengeRepository;

    private readonly IFireBaseService fireBaseService;

    public ChallengeServiceTest()
    {
        challengeRepository = A.Fake<IChallengeRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void ChallengeService_Add()
    {
        // Arrange
        string id = "id";
        CreateChallengeModel creation = A.Fake<CreateChallengeModel>();
        A.CallTo(() => challengeRepository.Add(A<Challenge>.Ignored)).Returns(new()
        {
            Id = id,
            Type = ChallengeType.Verify
        });
        var service = new ChallengeService(challengeRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ChallengeExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void ChallengeService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => challengeRepository.GetById(id)).Returns(new()
        {
            Id = id,
            StudentChallenges = new List<StudentChallenge>(),
        });
        A.CallTo(() => challengeRepository.Delete(id));
        var service = new ChallengeService(challengeRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void ChallengeService_GetAll()
    {
        // Arrange
        List<ChallengeType> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Challenge> pagedResultModel = new()
        {
            Result = new()
            {
                new()
                {
                    Type = ChallengeType.Verify
                },
                new()
                {
                    Type = ChallengeType.Verify
                },
                new()
                {
                    Type = ChallengeType.Verify
                },
            }
        };
        A.CallTo(() => challengeRepository.GetAll(typeIds, state, propertySort, isAsc,
            search, page, limit)).Returns(pagedResultModel);
        var service = new ChallengeService(challengeRepository, fireBaseService);

        // Act
        var result = service.GetAll(typeIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<ChallengeModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void ChallengeService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => challengeRepository.GetById(id))
            .Returns(new()
            {
                Id = id,
                Type = ChallengeType.Verify
            });
        var service = new ChallengeService(challengeRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ChallengeExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void ChallengeService_Update()
    {
        // Arrange
        string id = "id";
        string challengeName = "challengeName";
        UpdateChallengeModel update = A.Fake<UpdateChallengeModel>();
        A.CallTo(() => challengeRepository.GetById(id));
        A.CallTo(() => challengeRepository.Update(A<Challenge>.Ignored))
            .Returns(new()
            {
                Id = id,
                ChallengeName = challengeName,
                Type = ChallengeType.Verify
            });
        var service = new ChallengeService(challengeRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ChallengeExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(challengeName, result.Result.ChallengeName);
    }
}
