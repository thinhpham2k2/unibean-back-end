using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class ChallengeRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        Array values = Enum.GetValues(typeof(ChallengeType));
        for (int i = 1; i <= 10; i++)
        {
            Random random = new();
            ChallengeType randomType =
                (ChallengeType)values.GetValue(random.Next(values.Length));
            databaseContext.Challenges.Add(
            new Challenge()
            {
                Id = i.ToString(),
                Type = randomType,
                ChallengeName = "challengeName" + i,
                Amount = 100,
                Condition = 100,
                Image = "image" + i,
                FileName = "fileName" + i,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Description = "description" + i,
                State = true,
                Status = true,
            });
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }

    [Fact]
    public async void ChallengeRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Challenge>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ChallengeRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Challenges.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void ChallengeRepository_GetAll()
    {
        // Arrange
        List<ChallengeType> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeRepository(dbContext);

        // Act
        var result = repository.GetAll(typeIds, state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Challenge>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void ChallengeRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Challenge>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ChallengeRepository_Update()
    {
        // Arrange
        string id = "1";
        string challengeName = "challengeName";
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Challenges.FindAsync(id);
        existingAccount.ChallengeName = challengeName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Challenge>();
        Assert.Equal(id, result.Id);
        Assert.Equal(challengeName, result.ChallengeName);
    }
}
