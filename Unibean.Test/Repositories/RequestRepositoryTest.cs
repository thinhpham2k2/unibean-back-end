using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class RequestRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Requests.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Requests.Add(
                new Request()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    Brand = new()
                    {
                        Id = i.ToString(),
                        Status = true,
                        Wallets = new List<Wallet>()
                        {
                            new()
                            {
                                Id = i.ToString(),
                                Type = WalletType.Green,
                                Balance = 100,
                                Status = true,
                            }
                        }
                    },
                    AdminId = i.ToString(),
                    Amount = 100,
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
    public async void RequestRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new RequestRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            BrandId = "1",
            Amount = 100,
            Description = "description" + id,
            State = true,
            Status = true,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Request>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void RequestRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new RequestRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Requests.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void RequestRepository_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> adminIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new RequestRepository(dbContext);

        // Act
        var result = repository.GetAll(brandIds, adminIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Request>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void RequestRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new RequestRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Request>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void RequestRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new RequestRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Requests.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Request>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
