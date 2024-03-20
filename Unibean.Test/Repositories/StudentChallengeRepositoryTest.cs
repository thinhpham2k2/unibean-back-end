using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class StudentChallengeRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.StudentChallenges.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.StudentChallenges.Add(
                new StudentChallenge()
                {
                    Id = i.ToString(),
                    ChallengeId = i.ToString(),
                    StudentId = i.ToString(),
                    Amount = 100,
                    Current = i,
                    Condition = 100,
                    IsCompleted = false,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void StudentChallengeRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new StudentChallengeRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<StudentChallenge>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StudentChallengeRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentChallengeRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.StudentChallenges.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void StudentChallengeRepository_GetAll()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> challengeIds = new();
        List<ChallengeType> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new StudentChallengeRepository(dbContext);

        // Act
        var result = repository.GetAll(studentIds, challengeIds, typeIds,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<StudentChallenge>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void StudentChallengeRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentChallengeRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<StudentChallenge>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StudentChallengeRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentChallengeRepository(dbContext);

        // Act
        var existingAccount = await dbContext.StudentChallenges.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<StudentChallenge>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
