using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class StationRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Stations.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Stations.Add(
                new Station()
                {
                    Id = i.ToString(),
                    StationName = "areaName" + i,
                    Image = "image" + i,
                    FileName = "fileName" + i,
                    Address = "address" + i,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    State = StationState.Active,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void StationRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new StationRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Station>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StationRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StationRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Stations.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void StationRepository_GetAll()
    {
        // Arrange
        List<StationState> stateIds = new();
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new StationRepository(dbContext);

        // Act
        var result = repository.GetAll(stateIds, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Station>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void StationRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StationRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Station>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StationRepository_Update()
    {
        // Arrange
        string id = "1";
        string areaName = "areaName";
        var dbContext = await UnibeanDBContext();
        var repository = new StationRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Stations.FindAsync(id);
        existingAccount.StationName = areaName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Station>();
        Assert.Equal(id, result.Id);
        Assert.Equal(areaName, result.StationName);
    }
}
