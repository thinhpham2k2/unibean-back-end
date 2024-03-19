using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class StaffRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Staffs.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Staffs.Add(
                new Staff()
                {
                    Id = i.ToString(),
                    StationId = i.ToString(),
                    AccountId = i.ToString(),
                    FullName = "fullName" + i,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    State = true,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void StaffRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new StaffRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Staff>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StaffRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StaffRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Staffs.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void StaffRepository_GetAll()
    {
        // Arrange
        List<string> stationIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new StaffRepository(dbContext);

        // Act
        var result = repository.GetAll(stationIds, state, 
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Staff>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void StaffRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StaffRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Staff>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StaffRepository_Update()
    {
        // Arrange
        string id = "1";
        string fullName = "fullName";
        var dbContext = await UnibeanDBContext();
        var repository = new StaffRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Staffs.FindAsync(id);
        existingAccount.FullName = fullName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Staff>();
        Assert.Equal(id, result.Id);
        Assert.Equal(fullName, result.FullName);
    }
}
