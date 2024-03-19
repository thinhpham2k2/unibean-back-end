using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class AreaRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Areas.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Areas.Add(
                new Area()
                {
                    Id = i.ToString(),
                    AreaName = "areaName" + i,
                    Image = "image" + i,
                    FileName = "fileName" + i,
                    Address = "address" + i,
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
    public async void AreaRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new AreaRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Area>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void AreaRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new AreaRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Areas.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void AreaRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new AreaRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Area>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void AreaRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new AreaRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Area>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void AreaRepository_Update()
    {
        // Arrange
        string id = "1";
        string areaName = "areaName";
        var dbContext = await UnibeanDBContext();
        var repository = new AreaRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Areas.FindAsync(id);
        existingAccount.AreaName = areaName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Area>();
        Assert.Equal(id, result.Id);
        Assert.Equal(areaName, result.AreaName);
    }
}
