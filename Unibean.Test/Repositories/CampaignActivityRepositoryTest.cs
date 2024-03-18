using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class CampaignActivityRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.CampaignActivities.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.CampaignActivities.Add(
                new CampaignActivity()
                {
                    Id = i.ToString(),
                    CampaignId = i.ToString(),
                    DateCreated = DateTime.Now,
                    State = CampaignState.Pending,
                    Description = "description" + i,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void CampaignActivityRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignActivityRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignActivity>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignActivityRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignActivityRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.CampaignActivities.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void CampaignActivityRepository_GetAll()
    {
        // Arrange
        List<string> campaignIds = new();
        List<CampaignState> stateIds = new();
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignActivityRepository(dbContext);

        // Act
        var result = repository.GetAll(campaignIds, stateIds, propertySort, 
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<CampaignActivity>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void CampaignActivityRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignActivityRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignActivity>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.CampaignId);
    }

    [Fact]
    public async void CampaignActivityRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignActivityRepository(dbContext);

        // Act
        var existingAccount = await dbContext.CampaignActivities.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignActivity>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
