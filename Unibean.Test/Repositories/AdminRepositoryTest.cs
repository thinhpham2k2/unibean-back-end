using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class AdminRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Admins.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Admins.Add(
                new Admin()
                {
                    Id = i.ToString(),
                    AccountId = i.ToString(),
                    FullName = "fullname" + i,
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
    public async void AdminRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new AdminRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Admin>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void AdminRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new AdminRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Admins.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void AdminRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new AdminRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort, isAsc, 
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Admin>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void AdminRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new AdminRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Admin>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.AccountId);
    }

    [Fact]
    public async void AdminRepository_Update()
    {
        // Arrange
        string id = "1";
        string fullname = "fullname";
        var dbContext = await UnibeanDBContext();
        var repository = new AdminRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Admins.FindAsync(id);
        existingAccount.FullName = fullname;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Admin>();
        Assert.Equal(id, result.Id);
        Assert.Equal(fullname, result.FullName);
    }
}
